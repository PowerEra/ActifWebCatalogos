using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class PeriodosCerradosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PeriodosCerradosController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: PeriodosCerrados
        public async Task<IActionResult> Index()
        {
            var periodosCerrados = await _context.PeriodosCerrados
                .Include(p => p.Compania)
                .Include(p => p.TipoDepreciacion)
                .ToListAsync();
            return View(periodosCerrados);
        }

        // GET: PeriodosCerrados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var periodosCerrados = await _context.PeriodosCerrados
                .Include(p => p.Compania)
                .Include(p => p.TipoDepreciacion)
                .FirstOrDefaultAsync(m => m.IdCierreCompania == id);
            if (periodosCerrados == null)
            {
                return NotFound();
            }

            return View(periodosCerrados);
        }

        // GET: PeriodosCerrados/Create
        public IActionResult Create()
        {
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre");
            ViewData["IdTipoDep"] = new SelectList(_context.TipoDepreciacion, "IdTipoDep", "Descripcion");
            return View();
        }

        // POST: PeriodosCerrados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCierreCompania,Anio,Mes,IdCompania,FechaCierre,IdUsuarioCierre,IdTipoDep")] PeriodosCerrados periodosCerrados)
        {
            if (ModelState.IsValid)
            {
                _context.Add(periodosCerrados);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre", periodosCerrados.IdCompania);
            ViewData["IdTipoDep"] = new SelectList(_context.TipoDepreciacion, "IdTipoDep", "Descripcion", periodosCerrados.IdTipoDep);
            return View(periodosCerrados);
        }

        // GET: PeriodosCerrados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var periodosCerrados = await _context.PeriodosCerrados.FindAsync(id);
            if (periodosCerrados == null)
            {
                return NotFound();
            }
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre", periodosCerrados.IdCompania);
            ViewData["IdTipoDep"] = new SelectList(_context.TipoDepreciacion, "IdTipoDep", "Descripcion", periodosCerrados.IdTipoDep);
            return View(periodosCerrados);
        }

        // POST: PeriodosCerrados/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCierreCompania,Anio,Mes,IdCompania,FechaCierre,IdUsuarioCierre,IdTipoDep")] PeriodosCerrados periodosCerrados)
        {
            if (id != periodosCerrados.IdCierreCompania)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(periodosCerrados);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeriodosCerradosExists(periodosCerrados.IdCierreCompania))
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
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre", periodosCerrados.IdCompania);
            ViewData["IdTipoDep"] = new SelectList(_context.TipoDepreciacion, "IdTipoDep", "Descripcion", periodosCerrados.IdTipoDep);
            return View(periodosCerrados);
        }

        // GET: PeriodosCerrados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var periodosCerrados = await _context.PeriodosCerrados
                .Include(p => p.Compania)
                .Include(p => p.TipoDepreciacion)
                .FirstOrDefaultAsync(m => m.IdCierreCompania == id);
            if (periodosCerrados == null)
            {
                return NotFound();
            }

            return View(periodosCerrados);
        }

        // POST: PeriodosCerrados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var periodosCerrados = await _context.PeriodosCerrados.FindAsync(id);
            if (periodosCerrados != null)
            {
                _context.PeriodosCerrados.Remove(periodosCerrados);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var periodosCerrados = await _context.PeriodosCerrados
                .Include(p => p.Compania)
                .Include(p => p.TipoDepreciacion)
                .ToListAsync();

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("PeriodosCerrados");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID Cierre Compania";
                worksheet.Cells[1, 2].Value = "Año";
                worksheet.Cells[1, 3].Value = "Mes";
                worksheet.Cells[1, 4].Value = "ID Compania";
                worksheet.Cells[1, 5].Value = "Nombre Compania";
                worksheet.Cells[1, 6].Value = "Fecha Cierre";
                worksheet.Cells[1, 7].Value = "ID Usuario Cierre";
                worksheet.Cells[1, 8].Value = "ID Tipo Depreciacion";
                worksheet.Cells[1, 9].Value = "Descripcion Tipo Depreciacion";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 9])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var periodo in periodosCerrados)
                {
                    worksheet.Cells[row, 1].Value = periodo.IdCierreCompania;
                    worksheet.Cells[row, 2].Value = periodo.Anio;
                    worksheet.Cells[row, 3].Value = periodo.Mes;
                    worksheet.Cells[row, 4].Value = periodo.IdCompania;
                    worksheet.Cells[row, 5].Value = periodo.Compania?.Nombre;
                    worksheet.Cells[row, 6].Value = periodo.FechaCierre?.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Cells[row, 7].Value = periodo.IdUsuarioCierre;
                    worksheet.Cells[row, 8].Value = periodo.IdTipoDep;
                    worksheet.Cells[row, 9].Value = periodo.TipoDepreciacion?.Descripcion;
                    row++;
                }

                // Set column widths manually (AutoFitColumns not supported on non-Windows)
                for (int col = 1; col <= 9; col++)
                {
                    worksheet.Column(col).Width = 20;
                }

                // Return the Excel file
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"PeriodosCerrados_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
        }

        private bool PeriodosCerradosExists(int id)
        {
            return _context.PeriodosCerrados.Any(e => e.IdCierreCompania == id);
        }
    }
}
