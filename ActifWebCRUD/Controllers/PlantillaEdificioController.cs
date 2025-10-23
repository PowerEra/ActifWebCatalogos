using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using ActifWebCRUD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class PlantillaEdificioController : Controller
    {
        private readonly CookieAuthenticationService _authService;
        private readonly ApplicationDbContext _context;

        public PlantillaEdificioController(CookieAuthenticationService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        // GET: PlantillaEdificio
        public async Task<IActionResult> Index()
        {
            var user = _authService.GetUserFromCookie(HttpContext);

            if (user == null)
            {
                return View(new List<PlantillaEdificio>());
            }

            var plantillas = await _context.PlantillaEdificio
                .Where(a => a.IdCompania == user.IdCompania)
                .ToListAsync();
            return View(plantillas);
        }

        // GET: PlantillaEdificio/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantilla = await _context.PlantillaEdificio.FirstOrDefaultAsync(m => m.IdPlantilla == id);
            if (plantilla == null)
            {
                return NotFound();
            }

            // Manually load foreign key relations
            if (plantilla.IdTipoDep.HasValue)
            {
                plantilla.TipoDepreciacion = await _context.TipoDepreciacion.FindAsync(plantilla.IdTipoDep.Value);
            }

            if (plantilla.IdCompania.HasValue)
            {
                plantilla.Compania = await _context.Compania.FindAsync(plantilla.IdCompania.Value);
            }

            return View(plantilla);
        }

        // GET: PlantillaEdificio/Create
        public IActionResult Create()
        {
            PopulateTipoDepreciacionDropdown();
            PopulateCompaniaDropdown();
            return View();
        }

        // POST: PlantillaEdificio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Plantilla,IdTipoDep,TomaEdificio,TomaCC,IdCompania")] PlantillaEdificio plantilla)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plantilla);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateTipoDepreciacionDropdown(plantilla.IdTipoDep);
            PopulateCompaniaDropdown(plantilla.IdCompania);
            return View(plantilla);
        }

        // GET: PlantillaEdificio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantilla = await _context.PlantillaEdificio.FindAsync(id);
            if (plantilla == null)
            {
                return NotFound();
            }
            PopulateTipoDepreciacionDropdown(plantilla.IdTipoDep);
            PopulateCompaniaDropdown(plantilla.IdCompania);
            return View(plantilla);
        }

        // POST: PlantillaEdificio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPlantilla,Plantilla,IdTipoDep,TomaEdificio,TomaCC,IdCompania")] PlantillaEdificio plantilla)
        {
            if (id != plantilla.IdPlantilla)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plantilla);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantillaEdificioExists(plantilla.IdPlantilla))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateTipoDepreciacionDropdown(plantilla.IdTipoDep);
            PopulateCompaniaDropdown(plantilla.IdCompania);
            return View(plantilla);
        }

        // GET: PlantillaEdificio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantilla = await _context.PlantillaEdificio.FirstOrDefaultAsync(m => m.IdPlantilla == id);
            if (plantilla == null)
            {
                return NotFound();
            }

            // Manually load foreign key relations
            if (plantilla.IdTipoDep.HasValue)
            {
                plantilla.TipoDepreciacion = await _context.TipoDepreciacion.FindAsync(plantilla.IdTipoDep.Value);
            }

            if (plantilla.IdCompania.HasValue)
            {
                plantilla.Compania = await _context.Compania.FindAsync(plantilla.IdCompania.Value);
            }

            return View(plantilla);
        }

        // POST: PlantillaEdificio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plantilla = await _context.PlantillaEdificio.FindAsync(id);
            if (plantilla != null)
            {
                _context.PlantillaEdificio.Remove(plantilla);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var plantillas = await _context.PlantillaEdificio.ToListAsync();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("PlantillaEdificio");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID Plantilla";
                worksheet.Cells[1, 2].Value = "Plantilla";
                worksheet.Cells[1, 3].Value = "ID Tipo Depreciación";
                worksheet.Cells[1, 4].Value = "Toma Edificio";
                worksheet.Cells[1, 5].Value = "Toma Centro Costo";
                worksheet.Cells[1, 6].Value = "ID Compañía";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var plantilla in plantillas)
                {
                    worksheet.Cells[row, 1].Value = plantilla.IdPlantilla;
                    worksheet.Cells[row, 2].Value = plantilla.Plantilla;
                    worksheet.Cells[row, 3].Value = plantilla.IdTipoDep;
                    worksheet.Cells[row, 4].Value = plantilla.TomaEdificio.HasValue ? (plantilla.TomaEdificio.Value ? "Sí" : "No") : "";
                    worksheet.Cells[row, 5].Value = plantilla.TomaCC.HasValue ? (plantilla.TomaCC.Value ? "Sí" : "No") : "";
                    worksheet.Cells[row, 6].Value = plantilla.IdCompania;
                    row++;
                }

                // Set column widths manually (AutoFitColumns not supported on non-Windows)
                for (int col = 1; col <= 6; col++)
                {
                    worksheet.Column(col).Width = 20;
                }

                var fileName = $"PlantillaEdificio_{DateTime.Now:yyyyMMdd}.xlsx";
                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        private void PopulateTipoDepreciacionDropdown(object? selectedValue = null)
        {
            var tiposDepreciacion = _context.TipoDepreciacion
                .OrderBy(t => t.Descripcion)
                .Select(t => new SelectListItem
                {
                    Value = t.IdTipoDep.ToString(),
                    Text = t.Descripcion
                })
                .ToList();

            tiposDepreciacion.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione un Tipo de Depreciación --" });

            ViewBag.IdTipoDep = new SelectList(tiposDepreciacion, "Value", "Text", selectedValue);
        }

        private void PopulateCompaniaDropdown(object? selectedValue = null)
        {
            var companias = _context.Compania
                .OrderBy(c => c.Nombre)
                .Select(c => new SelectListItem
                {
                    Value = c.IdCompania.ToString(),
                    Text = c.Nombre
                })
                .ToList();

            companias.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione una Compañía --" });

            ViewBag.IdCompania = new SelectList(companias, "Value", "Text", selectedValue);
        }

        private bool PlantillaEdificioExists(int id)
        {
            return _context.PlantillaEdificio.Any(e => e.IdPlantilla == id);
        }
    }
}
