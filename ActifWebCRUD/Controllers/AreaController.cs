using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class AreaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AreaController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Area
        public async Task<IActionResult> Index()
        {
            var areas = await _context.Area.ToListAsync();
            return View(areas);
        }

        // GET: Area/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _context.Area
                .FirstOrDefaultAsync(m => m.IdArea == id);
            if (area == null)
            {
                return NotFound();
            }

            return View(area);
        }

        // GET: Area/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Area/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdArea,Descripcion,Cta1,Cta2,Cta3,Cta4,Cta5,Cta6,IdEdificio,IdPiso,IdAreaOrig,IdCompania")] Area area)
        {
            if (ModelState.IsValid)
            {
                _context.Add(area);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(area);
        }

        // GET: Area/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _context.Area.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }
            return View(area);
        }

        // POST: Area/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdArea,Descripcion,Cta1,Cta2,Cta3,Cta4,Cta5,Cta6,IdEdificio,IdPiso,IdAreaOrig,IdCompania")] Area area)
        {
            if (id != area.IdArea)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the original rv value
                    var original = await _context.Area.AsNoTracking().FirstOrDefaultAsync(a => a.IdArea == id);
                    if (original != null)
                    {
                        area.Rv = original.Rv;
                    }

                    _context.Update(area);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AreaExists(area.IdArea))
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
            return View(area);
        }

        // GET: Area/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _context.Area
                .FirstOrDefaultAsync(m => m.IdArea == id);
            if (area == null)
            {
                return NotFound();
            }

            return View(area);
        }

        // POST: Area/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var area = await _context.Area.FindAsync(id);
            if (area != null)
            {
                _context.Area.Remove(area);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var areas = await _context.Area.ToListAsync();

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Areas");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID Area";
                worksheet.Cells[1, 2].Value = "Descripcion";
                worksheet.Cells[1, 3].Value = "Cuenta 1";
                worksheet.Cells[1, 4].Value = "Cuenta 2";
                worksheet.Cells[1, 5].Value = "Cuenta 3";
                worksheet.Cells[1, 6].Value = "Cuenta 4";
                worksheet.Cells[1, 7].Value = "Cuenta 5";
                worksheet.Cells[1, 8].Value = "Cuenta 6";
                worksheet.Cells[1, 9].Value = "ID Edificio";
                worksheet.Cells[1, 10].Value = "ID Piso";
                worksheet.Cells[1, 11].Value = "ID Area Origen";
                worksheet.Cells[1, 12].Value = "ID Compania";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 12])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var area in areas)
                {
                    worksheet.Cells[row, 1].Value = area.IdArea;
                    worksheet.Cells[row, 2].Value = area.Descripcion;
                    worksheet.Cells[row, 3].Value = area.Cta1;
                    worksheet.Cells[row, 4].Value = area.Cta2;
                    worksheet.Cells[row, 5].Value = area.Cta3;
                    worksheet.Cells[row, 6].Value = area.Cta4;
                    worksheet.Cells[row, 7].Value = area.Cta5;
                    worksheet.Cells[row, 8].Value = area.Cta6;
                    worksheet.Cells[row, 9].Value = area.IdEdificio;
                    worksheet.Cells[row, 10].Value = area.IdPiso;
                    worksheet.Cells[row, 11].Value = area.IdAreaOrig;
                    worksheet.Cells[row, 12].Value = area.IdCompania;
                    row++;
                }

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Return the Excel file
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Areas_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
        }

        private bool AreaExists(int id)
        {
            return _context.Area.Any(e => e.IdArea == id);
        }
    }
}
