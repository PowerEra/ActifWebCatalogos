using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class SignificadoIdController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public SignificadoIdController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: SignificadoId
        public async Task<IActionResult> Index()
        {
            var significadosId = await _context.SignificadoId.ToListAsync();
            return View(significadosId);
        }

        // GET: SignificadoId/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var significadoId = await _context.SignificadoId
                .FirstOrDefaultAsync(m => m.IdSignificadoId == id);
            if (significadoId == null)
            {
                return NotFound();
            }

            return View(significadoId);
        }

        // GET: SignificadoId/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SignificadoId/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Descripcion")] SignificadoId significadoId)
        {
            if (ModelState.IsValid)
            {
                _context.Add(significadoId);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(significadoId);
        }

        // GET: SignificadoId/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var significadoId = await _context.SignificadoId.FindAsync(id);
            if (significadoId == null)
            {
                return NotFound();
            }
            return View(significadoId);
        }

        // POST: SignificadoId/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("IdSignificadoId,Descripcion")] SignificadoId significadoId)
        {
            if (id != significadoId.IdSignificadoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the original rv value
                    var original = await _context.SignificadoId.AsNoTracking().FirstOrDefaultAsync(e => e.IdSignificadoId == id);
                    if (original != null)
                    {
                        significadoId.Rv = original.Rv;
                    }

                    _context.Update(significadoId);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SignificadoIdExists(significadoId.IdSignificadoId))
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
            return View(significadoId);
        }

        // GET: SignificadoId/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var significadoId = await _context.SignificadoId
                .FirstOrDefaultAsync(m => m.IdSignificadoId == id);
            if (significadoId == null)
            {
                return NotFound();
            }

            return View(significadoId);
        }

        // POST: SignificadoId/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var significadoId = await _context.SignificadoId.FindAsync(id);
            if (significadoId != null)
            {
                _context.SignificadoId.Remove(significadoId);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var significadosId = await _context.SignificadoId.ToListAsync();

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("SignificadoId");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID Significado ID";
                worksheet.Cells[1, 2].Value = "Descripcion";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 2])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var item in significadosId)
                {
                    worksheet.Cells[row, 1].Value = item.IdSignificadoId;
                    worksheet.Cells[row, 2].Value = item.Descripcion;
                    row++;
                }

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Return the Excel file
                var fileName = $"SignificadoId_{DateTime.Now:yyyyMMdd}.xlsx";
                return File(package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
        }

        private bool SignificadoIdExists(short id)
        {
            return _context.SignificadoId.Any(e => e.IdSignificadoId == id);
        }
    }
}
