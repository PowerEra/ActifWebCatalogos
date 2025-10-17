using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class MonedaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MonedaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Moneda
        public async Task<IActionResult> Index()
        {
            var monedas = await _context.Moneda
                .Include(m => m.Pais)
                .OrderBy(m => m.IdMoneda)
                .ToListAsync();
            return View(monedas);
        }

        // GET: Moneda/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moneda = await _context.Moneda
                .Include(m => m.Pais)
                .FirstOrDefaultAsync(m => m.IdMoneda == id);

            if (moneda == null)
            {
                return NotFound();
            }

            return View(moneda);
        }

        // GET: Moneda/Create
        public IActionResult Create()
        {
            ViewData["IdPais"] = new SelectList(_context.Pais.OrderBy(p => p.Nombre), "IdPais", "Nombre");
            return View();
        }

        // POST: Moneda/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPais,Nombre,Simbolo")] Moneda moneda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(moneda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPais"] = new SelectList(_context.Pais.OrderBy(p => p.Nombre), "IdPais", "Nombre", moneda.IdPais);
            return View(moneda);
        }

        // GET: Moneda/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moneda = await _context.Moneda.FindAsync(id);
            if (moneda == null)
            {
                return NotFound();
            }
            ViewData["IdPais"] = new SelectList(_context.Pais.OrderBy(p => p.Nombre), "IdPais", "Nombre", moneda.IdPais);
            return View(moneda);
        }

        // POST: Moneda/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("IdMoneda,IdPais,Nombre,Simbolo,Rv")] Moneda moneda)
        {
            if (id != moneda.IdMoneda)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moneda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonedaExists(moneda.IdMoneda))
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
            ViewData["IdPais"] = new SelectList(_context.Pais.OrderBy(p => p.Nombre), "IdPais", "Nombre", moneda.IdPais);
            return View(moneda);
        }

        // GET: Moneda/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moneda = await _context.Moneda
                .Include(m => m.Pais)
                .FirstOrDefaultAsync(m => m.IdMoneda == id);

            if (moneda == null)
            {
                return NotFound();
            }

            return View(moneda);
        }

        // POST: Moneda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var moneda = await _context.Moneda.FindAsync(id);
            if (moneda != null)
            {
                _context.Moneda.Remove(moneda);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Moneda/ExportToExcel
        public async Task<IActionResult> ExportToExcel()
        {
            var monedas = await _context.Moneda
                .Include(m => m.Pais)
                .OrderBy(m => m.IdMoneda)
                .ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Monedas");

                // Headers
                worksheet.Cells[1, 1].Value = "ID Moneda";
                worksheet.Cells[1, 2].Value = "País";
                worksheet.Cells[1, 3].Value = "Nombre";
                worksheet.Cells[1, 4].Value = "Símbolo";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 4])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Data
                int row = 2;
                foreach (var moneda in monedas)
                {
                    worksheet.Cells[row, 1].Value = moneda.IdMoneda;
                    worksheet.Cells[row, 2].Value = moneda.Pais?.Nombre ?? "";
                    worksheet.Cells[row, 3].Value = moneda.Nombre ?? "";
                    worksheet.Cells[row, 4].Value = moneda.Simbolo ?? "";
                    row++;
                }

                // Manually set column widths (AutoFit has issues on macOS)
                worksheet.Column(1).Width = 12; // ID
                worksheet.Column(2).Width = 25; // País
                worksheet.Column(3).Width = 25; // Nombre
                worksheet.Column(4).Width = 12; // Símbolo

                var fileName = "Monedas_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
                return File(package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
        }

        private bool MonedaExists(short id)
        {
            return _context.Moneda.Any(e => e.IdMoneda == id);
        }
    }
}
