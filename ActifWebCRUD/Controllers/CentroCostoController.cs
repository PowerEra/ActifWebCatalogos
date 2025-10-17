using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class CentroCostoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public CentroCostoController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: CentroCosto
        public async Task<IActionResult> Index()
        {
            // Use the VIEW for SELECT operations
            var centrosCosto = await _context.VCentroCosto.ToListAsync();
            return View(centrosCosto);
        }

        // GET: CentroCosto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centroCosto = await _context.CentroCosto
                .Include(c => c.Compania)
                .FirstOrDefaultAsync(m => m.IdCentroCosto == id);
            if (centroCosto == null)
            {
                return NotFound();
            }

            return View(centroCosto);
        }

        // GET: CentroCosto/Create
        public IActionResult Create()
        {
            ViewBag.IdCompania = new SelectList(_context.Compania, "IdCompania", "Nombre");
            return View();
        }

        // POST: CentroCosto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCentroCosto,IdCompania,Codigo,Descripcion,Responsable,Status,Cta1,Cta2,Cta3,Cta4,Cta5,Cta6,Prorra,TransEntra,TransSale,Bajas,Altas,Cta11,Cta12,Cta13,Cta14,Cta15,Cta16")] CentroCosto centroCosto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(centroCosto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.IdCompania = new SelectList(_context.Compania, "IdCompania", "Nombre", centroCosto.IdCompania);
            return View(centroCosto);
        }

        // GET: CentroCosto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centroCosto = await _context.CentroCosto.FindAsync(id);
            if (centroCosto == null)
            {
                return NotFound();
            }
            ViewBag.IdCompania = new SelectList(_context.Compania, "IdCompania", "Nombre", centroCosto.IdCompania);
            return View(centroCosto);
        }

        // POST: CentroCosto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCentroCosto,IdCompania,Codigo,Descripcion,Responsable,Status,Cta1,Cta2,Cta3,Cta4,Cta5,Cta6,Prorra,TransEntra,TransSale,Bajas,Altas,Cta11,Cta12,Cta13,Cta14,Cta15,Cta16")] CentroCosto centroCosto)
        {
            if (id != centroCosto.IdCentroCosto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the original rv value
                    var original = await _context.CentroCosto.AsNoTracking().FirstOrDefaultAsync(c => c.IdCentroCosto == id);
                    if (original != null)
                    {
                        centroCosto.Rv = original.Rv;
                    }

                    _context.Update(centroCosto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CentroCostoExists(centroCosto.IdCentroCosto))
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
            ViewBag.IdCompania = new SelectList(_context.Compania, "IdCompania", "Nombre", centroCosto.IdCompania);
            return View(centroCosto);
        }

        // GET: CentroCosto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centroCosto = await _context.CentroCosto
                .Include(c => c.Compania)
                .FirstOrDefaultAsync(m => m.IdCentroCosto == id);
            if (centroCosto == null)
            {
                return NotFound();
            }

            return View(centroCosto);
        }

        // POST: CentroCosto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var centroCosto = await _context.CentroCosto.FindAsync(id);
            if (centroCosto != null)
            {
                _context.CentroCosto.Remove(centroCosto);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var centrosCosto = await _context.VCentroCosto.ToListAsync();

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Centros de Costo");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID Centro Costo";
                worksheet.Cells[1, 2].Value = "ID Compañía";
                worksheet.Cells[1, 3].Value = "Compañía";
                worksheet.Cells[1, 4].Value = "Código";
                worksheet.Cells[1, 5].Value = "Descripción";
                worksheet.Cells[1, 6].Value = "Responsable";
                worksheet.Cells[1, 7].Value = "Status";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 7])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var centroCosto in centrosCosto)
                {
                    worksheet.Cells[row, 1].Value = centroCosto.IdCentroCosto;
                    worksheet.Cells[row, 2].Value = centroCosto.IdCompania;
                    worksheet.Cells[row, 3].Value = centroCosto.Compania;
                    worksheet.Cells[row, 4].Value = centroCosto.Codigo;
                    worksheet.Cells[row, 5].Value = centroCosto.Descripcion;
                    worksheet.Cells[row, 6].Value = centroCosto.Responsable;
                    worksheet.Cells[row, 7].Value = centroCosto.Status;
                    row++;
                }

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Return the Excel file
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"CentrosCosto_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
        }

        private bool CentroCostoExists(int id)
        {
            return _context.CentroCosto.Any(e => e.IdCentroCosto == id);
        }
    }
}
