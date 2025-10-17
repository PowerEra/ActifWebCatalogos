using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class TipoCalculoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipoCalculoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TipoCalculo
        public async Task<IActionResult> Index()
        {
            var tiposCalculo = await _context.TipoCalculo
                .OrderBy(t => t.IdTipoCalculo)
                .ToListAsync();
            return View(tiposCalculo);
        }

        // GET: TipoCalculo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoCalculo = await _context.TipoCalculo
                .FirstOrDefaultAsync(m => m.IdTipoCalculo == id);

            if (tipoCalculo == null)
            {
                return NotFound();
            }

            return View(tipoCalculo);
        }

        // GET: TipoCalculo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoCalculo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Descripcion")] TipoCalculo tipoCalculo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoCalculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoCalculo);
        }

        // GET: TipoCalculo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoCalculo = await _context.TipoCalculo.FindAsync(id);
            if (tipoCalculo == null)
            {
                return NotFound();
            }
            return View(tipoCalculo);
        }

        // POST: TipoCalculo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoCalculo,Descripcion")] TipoCalculo tipoCalculo)
        {
            if (id != tipoCalculo.IdTipoCalculo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoCalculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoCalculoExists(tipoCalculo.IdTipoCalculo))
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
            return View(tipoCalculo);
        }

        // GET: TipoCalculo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoCalculo = await _context.TipoCalculo
                .FirstOrDefaultAsync(m => m.IdTipoCalculo == id);

            if (tipoCalculo == null)
            {
                return NotFound();
            }

            return View(tipoCalculo);
        }

        // POST: TipoCalculo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoCalculo = await _context.TipoCalculo.FindAsync(id);
            if (tipoCalculo != null)
            {
                _context.TipoCalculo.Remove(tipoCalculo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: TipoCalculo/ExportToExcel
        public async Task<IActionResult> ExportToExcel()
        {
            var tiposCalculo = await _context.TipoCalculo
                .OrderBy(t => t.IdTipoCalculo)
                .ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("TiposCalculo");

                // Headers
                worksheet.Cells[1, 1].Value = "ID Tipo Cálculo";
                worksheet.Cells[1, 2].Value = "Descripción";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 2])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Data
                int row = 2;
                foreach (var tipoCalculo in tiposCalculo)
                {
                    worksheet.Cells[row, 1].Value = tipoCalculo.IdTipoCalculo;
                    worksheet.Cells[row, 2].Value = tipoCalculo.Descripcion ?? "";
                    row++;
                }

                // Manually set column widths (AutoFit has issues on macOS)
                worksheet.Column(1).Width = 20; // ID
                worksheet.Column(2).Width = 30; // Descripción

                var fileName = "TiposCalculo_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
                return File(package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
        }

        private bool TipoCalculoExists(int id)
        {
            return _context.TipoCalculo.Any(e => e.IdTipoCalculo == id);
        }
    }
}
