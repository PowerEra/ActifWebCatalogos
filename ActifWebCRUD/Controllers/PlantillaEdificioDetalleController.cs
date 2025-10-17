using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class PlantillaEdificioDetalleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PlantillaEdificioDetalleController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: PlantillaEdificioDetalle
        public async Task<IActionResult> Index()
        {
            var detalles = await _context.PlantillaEdificioDetalle.ToListAsync();

            // Manually load navigation properties
            var plantillaIds = detalles.Where(d => d.IdPlantilla.HasValue).Select(d => d.IdPlantilla.Value).Distinct().ToList();
            var companiaIds = detalles.Where(d => d.IdCompania.HasValue).Select(d => d.IdCompania.Value).Distinct().ToList();

            var plantillas = await _context.PlantillaEdificio.AsNoTracking().Where(p => plantillaIds.Contains(p.IdPlantilla)).ToListAsync();
            var companias = await _context.Compania.AsNoTracking().ToListAsync(); // Load all to avoid casting issues

            foreach (var detalle in detalles)
            {
                if (detalle.IdPlantilla.HasValue)
                {
                    detalle.PlantillaEdificio = plantillas.FirstOrDefault(p => p.IdPlantilla == detalle.IdPlantilla.Value);
                }
                if (detalle.IdCompania.HasValue)
                {
                    detalle.Compania = companias.FirstOrDefault(c => c.IdCompania == detalle.IdCompania.Value);
                }
            }

            return View(detalles);
        }

        // GET: PlantillaEdificioDetalle/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle = await _context.PlantillaEdificioDetalle
                .FirstOrDefaultAsync(m => m.IdPlantillaDetalle == id);
            if (detalle == null)
            {
                return NotFound();
            }

            // Manually load navigation properties (load in memory to avoid EF casting issues)
            if (detalle.IdPlantilla.HasValue)
            {
                var plantillas = await _context.PlantillaEdificio.ToListAsync();
                detalle.PlantillaEdificio = plantillas.FirstOrDefault(p => p.IdPlantilla == detalle.IdPlantilla.Value);
            }
            if (detalle.IdCompania.HasValue)
            {
                var companias = await _context.Compania.ToListAsync();
                detalle.Compania = companias.FirstOrDefault(c => c.IdCompania == detalle.IdCompania.Value);
            }

            return View(detalle);
        }

        // GET: PlantillaEdificioDetalle/Create
        public IActionResult Create()
        {
            ViewData["IdPlantilla"] = new SelectList(_context.PlantillaEdificio, "IdPlantilla", "Plantilla");
            ViewData["IdEdificio"] = new SelectList(_context.Edificio, "IdEdificio", "IdEdificio");
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre");
            return View();
        }

        // POST: PlantillaEdificioDetalle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPlantillaDetalle,IdPlantilla,IdEdificio,IdCompania")] PlantillaEdificioDetalle detalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPlantilla"] = new SelectList(_context.PlantillaEdificio, "IdPlantilla", "Plantilla", detalle.IdPlantilla);
            ViewData["IdEdificio"] = new SelectList(_context.Edificio, "IdEdificio", "IdEdificio", detalle.IdEdificio);
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre", detalle.IdCompania);
            return View(detalle);
        }

        // GET: PlantillaEdificioDetalle/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle = await _context.PlantillaEdificioDetalle.FindAsync(id);
            if (detalle == null)
            {
                return NotFound();
            }
            ViewData["IdPlantilla"] = new SelectList(_context.PlantillaEdificio, "IdPlantilla", "Plantilla", detalle.IdPlantilla);
            ViewData["IdEdificio"] = new SelectList(_context.Edificio, "IdEdificio", "IdEdificio", detalle.IdEdificio);
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre", detalle.IdCompania);
            return View(detalle);
        }

        // POST: PlantillaEdificioDetalle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPlantillaDetalle,IdPlantilla,IdEdificio,IdCompania")] PlantillaEdificioDetalle detalle)
        {
            if (id != detalle.IdPlantillaDetalle)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantillaEdificioDetalleExists(detalle.IdPlantillaDetalle))
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
            ViewData["IdPlantilla"] = new SelectList(_context.PlantillaEdificio, "IdPlantilla", "Plantilla", detalle.IdPlantilla);
            ViewData["IdEdificio"] = new SelectList(_context.Edificio, "IdEdificio", "IdEdificio", detalle.IdEdificio);
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre", detalle.IdCompania);
            return View(detalle);
        }

        // GET: PlantillaEdificioDetalle/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle = await _context.PlantillaEdificioDetalle
                .FirstOrDefaultAsync(m => m.IdPlantillaDetalle == id);
            if (detalle == null)
            {
                return NotFound();
            }

            // Manually load navigation properties (load in memory to avoid EF casting issues)
            if (detalle.IdPlantilla.HasValue)
            {
                var plantillas = await _context.PlantillaEdificio.ToListAsync();
                detalle.PlantillaEdificio = plantillas.FirstOrDefault(p => p.IdPlantilla == detalle.IdPlantilla.Value);
            }
            if (detalle.IdCompania.HasValue)
            {
                var companias = await _context.Compania.ToListAsync();
                detalle.Compania = companias.FirstOrDefault(c => c.IdCompania == detalle.IdCompania.Value);
            }

            return View(detalle);
        }

        // POST: PlantillaEdificioDetalle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalle = await _context.PlantillaEdificioDetalle.FindAsync(id);
            if (detalle != null)
            {
                _context.PlantillaEdificioDetalle.Remove(detalle);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var detalles = await _context.PlantillaEdificioDetalle.ToListAsync();

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("PlantillaEdificioDetalle");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID Plantilla Detalle";
                worksheet.Cells[1, 2].Value = "ID Plantilla";
                worksheet.Cells[1, 3].Value = "Plantilla";
                worksheet.Cells[1, 4].Value = "ID Edificio";
                worksheet.Cells[1, 5].Value = "ID Compania";
                worksheet.Cells[1, 6].Value = "Nombre Compania";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var detalle in detalles)
                {
                    worksheet.Cells[row, 1].Value = detalle.IdPlantillaDetalle;
                    worksheet.Cells[row, 2].Value = detalle.IdPlantilla;
                    
                    worksheet.Cells[row, 4].Value = detalle.IdEdificio;
                    worksheet.Cells[row, 5].Value = detalle.IdCompania;
                    row++;
                }

                // Load related data after the initial loop (avoid EF casting issues)
                var plantillaIds = detalles.Where(d => d.IdPlantilla.HasValue).Select(d => d.IdPlantilla.Value).Distinct().ToList();
                var companiaIds = detalles.Where(d => d.IdCompania.HasValue).Select(d => d.IdCompania.Value).Distinct().ToList();
                var plantillas = await _context.PlantillaEdificio.Where(p => plantillaIds.Contains(p.IdPlantilla)).ToListAsync();
                var companias = await _context.Compania.Where(c => companiaIds.Contains(c.IdCompania)).ToListAsync();

                // Go back and fill in the names
                row = 2;
                foreach (var detalle in detalles)
                {
                    var plantilla = detalle.IdPlantilla.HasValue ? plantillas.FirstOrDefault(p => p.IdPlantilla == detalle.IdPlantilla.Value) : null;
                    worksheet.Cells[row, 3].Value = plantilla?.Plantilla;

                    var compania = detalle.IdCompania.HasValue ? companias.FirstOrDefault(c => c.IdCompania == detalle.IdCompania.Value) : null;
                    worksheet.Cells[row, 6].Value = compania?.Nombre;
                    
                    row++;
                }

                // Set column widths manually (AutoFitColumns not supported on non-Windows)
                worksheet.Column(1).Width = 25;
                worksheet.Column(2).Width = 20;
                worksheet.Column(3).Width = 30;
                worksheet.Column(4).Width = 20;
                worksheet.Column(5).Width = 20;
                worksheet.Column(6).Width = 30;

                // Return the Excel file
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"PlantillaEdificioDetalle_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
        }

        private bool PlantillaEdificioDetalleExists(int id)
        {
            return _context.PlantillaEdificioDetalle.Any(e => e.IdPlantillaDetalle == id);
        }
    }
}
