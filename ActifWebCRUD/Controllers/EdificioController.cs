using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using ActifWebCRUD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class EdificioController : Controller
    {
        private readonly CookieAuthenticationService _authService;
        private readonly ApplicationDbContext _context;

        public EdificioController(CookieAuthenticationService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        // GET: Edificio
        public async Task<IActionResult> Index()
        {
            var user = _authService.GetUserFromCookie(HttpContext);

            if (user == null)
            {
                return View(new List<Edificio>());
            }

            // Use vEdificio view for Index display
            var edificios = await _context.VEdificio
                .Where(a => a.IdCompania == user.IdCompania)
                .ToListAsync();
            return View(edificios);
        }

        // GET: Edificio/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var edificio = await _context.Edificio.FirstOrDefaultAsync(m => m.IdEdificio == id);
            if (edificio == null)
            {
                return NotFound();
            }

            // Manually load foreign key relations
            if (edificio.IdCompania != 0)
            {
                edificio.Compania = await _context.Compania.FindAsync(edificio.IdCompania);
            }

            if (edificio.IdEstado.HasValue)
            {
                edificio.Estado = await _context.Estado.FindAsync(edificio.IdEstado.Value);
            }

            return View(edificio);
        }

        // GET: Edificio/Create
        public IActionResult Create()
        {
            PopulateCompaniaDropdown();
            PopulateEstadoDropdown();
            return View();
        }

        // POST: Edificio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCompania,Descripcion,CalleNumero,Colonia,DelegMpio,CodigoPostal,IdEstado,Telefono,Cta1,Cta2,Cta3,Cta4,Cta5,Cta6,Inactivo,Ciudad,IdEdificioOrig,IdCompaniaOrig")] Edificio edificio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(edificio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateCompaniaDropdown(edificio.IdCompania);
            PopulateEstadoDropdown(edificio.IdEstado);
            return View(edificio);
        }

        // GET: Edificio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var edificio = await _context.Edificio.FindAsync(id);
            if (edificio == null)
            {
                return NotFound();
            }
            PopulateCompaniaDropdown(edificio.IdCompania);
            PopulateEstadoDropdown(edificio.IdEstado);
            return View(edificio);
        }

        // POST: Edificio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEdificio,IdCompania,Descripcion,CalleNumero,Colonia,DelegMpio,CodigoPostal,IdEstado,Telefono,Cta1,Cta2,Cta3,Cta4,Cta5,Cta6,Inactivo,Ciudad,IdEdificioOrig,IdCompaniaOrig")] Edificio edificio)
        {
            if (id != edificio.IdEdificio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(edificio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EdificioExists(edificio.IdEdificio))
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
            PopulateCompaniaDropdown(edificio.IdCompania);
            PopulateEstadoDropdown(edificio.IdEstado);
            return View(edificio);
        }

        // GET: Edificio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var edificio = await _context.Edificio.FirstOrDefaultAsync(m => m.IdEdificio == id);
            if (edificio == null)
            {
                return NotFound();
            }

            // Manually load foreign key relations
            if (edificio.IdCompania != 0)
            {
                edificio.Compania = await _context.Compania.FindAsync(edificio.IdCompania);
            }

            if (edificio.IdEstado.HasValue)
            {
                edificio.Estado = await _context.Estado.FindAsync(edificio.IdEstado.Value);
            }

            return View(edificio);
        }

        // POST: Edificio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var edificio = await _context.Edificio.FindAsync(id);
            if (edificio != null)
            {
                _context.Edificio.Remove(edificio);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var edificios = await _context.VEdificio.ToListAsync();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Edificios");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID Edificio";
                worksheet.Cells[1, 2].Value = "ID Compañía";
                worksheet.Cells[1, 3].Value = "Compañía";
                worksheet.Cells[1, 4].Value = "Descripción";
                worksheet.Cells[1, 5].Value = "Calle y Número";
                worksheet.Cells[1, 6].Value = "Colonia";
                worksheet.Cells[1, 7].Value = "Delegación/Municipio";
                worksheet.Cells[1, 8].Value = "Código Postal";
                worksheet.Cells[1, 9].Value = "ID Estado";
                worksheet.Cells[1, 10].Value = "Teléfono";
                worksheet.Cells[1, 11].Value = "Inactivo";
                worksheet.Cells[1, 12].Value = "ID Edificio Original";
                worksheet.Cells[1, 13].Value = "Cuenta 2";
                worksheet.Cells[1, 14].Value = "Cuenta 3";
                worksheet.Cells[1, 15].Value = "Cuenta 4";
                worksheet.Cells[1, 16].Value = "Cuenta 5";
                worksheet.Cells[1, 17].Value = "Cuenta 6";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 17])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var edificio in edificios)
                {
                    worksheet.Cells[row, 1].Value = edificio.IdEdificio;
                    worksheet.Cells[row, 2].Value = edificio.IdCompania;
                    worksheet.Cells[row, 3].Value = edificio.Compania;
                    worksheet.Cells[row, 4].Value = edificio.Descripcion;
                    worksheet.Cells[row, 5].Value = edificio.CalleNumero;
                    worksheet.Cells[row, 6].Value = edificio.Colonia;
                    worksheet.Cells[row, 7].Value = edificio.DelegMpio;
                    worksheet.Cells[row, 8].Value = edificio.CodigoPostal;
                    worksheet.Cells[row, 9].Value = edificio.IdEstado;
                    worksheet.Cells[row, 10].Value = edificio.Telefono;
                    worksheet.Cells[row, 11].Value = edificio.Inactivo;
                    worksheet.Cells[row, 12].Value = edificio.IdEdificioOrig;
                    worksheet.Cells[row, 13].Value = edificio.Cta2;
                    worksheet.Cells[row, 14].Value = edificio.Cta3;
                    worksheet.Cells[row, 15].Value = edificio.Cta4;
                    worksheet.Cells[row, 16].Value = edificio.Cta5;
                    worksheet.Cells[row, 17].Value = edificio.Cta6;
                    row++;
                }

                // Set column widths manually (AutoFitColumns not supported on non-Windows)
                for (int col = 1; col <= 17; col++)
                {
                    worksheet.Column(col).Width = 15;
                }

                var fileName = $"Edificios_{DateTime.Now:yyyyMMdd}.xlsx";
                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
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

        private void PopulateEstadoDropdown(object? selectedValue = null)
        {
            var estados = _context.Estado
                .OrderBy(e => e.Nombre)
                .Select(e => new SelectListItem
                {
                    Value = e.IdEstado.ToString(),
                    Text = e.Nombre
                })
                .ToList();

            estados.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione un Estado --" });

            ViewBag.IdEstado = new SelectList(estados, "Value", "Text", selectedValue);
        }

        private bool EdificioExists(int id)
        {
            return _context.Edificio.Any(e => e.IdEdificio == id);
        }
    }
}
