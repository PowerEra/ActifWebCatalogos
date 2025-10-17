using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class ClaseTipoCambioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClaseTipoCambioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClaseTipoCambio
        public async Task<IActionResult> Index()
        {
            var clasesTipoCambio = await _context.ClaseTipoCambio
                .OrderBy(c => c.IdClaseTipCam)
                .ToListAsync();
            return View(clasesTipoCambio);
        }

        // GET: ClaseTipoCambio/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claseTipoCambio = await _context.ClaseTipoCambio
                .FirstOrDefaultAsync(m => m.IdClaseTipCam == id);

            if (claseTipoCambio == null)
            {
                return NotFound();
            }

            return View(claseTipoCambio);
        }

        // GET: ClaseTipoCambio/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClaseTipoCambio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdClaseTipCam,Descripcion")] ClaseTipoCambio claseTipoCambio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(claseTipoCambio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(claseTipoCambio);
        }

        // GET: ClaseTipoCambio/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claseTipoCambio = await _context.ClaseTipoCambio.FindAsync(id);
            if (claseTipoCambio == null)
            {
                return NotFound();
            }
            return View(claseTipoCambio);
        }

        // POST: ClaseTipoCambio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("IdClaseTipCam,Descripcion")] ClaseTipoCambio claseTipoCambio)
        {
            if (id != claseTipoCambio.IdClaseTipCam)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(claseTipoCambio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClaseTipoCambioExists(claseTipoCambio.IdClaseTipCam))
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
            return View(claseTipoCambio);
        }

        // GET: ClaseTipoCambio/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claseTipoCambio = await _context.ClaseTipoCambio
                .FirstOrDefaultAsync(m => m.IdClaseTipCam == id);

            if (claseTipoCambio == null)
            {
                return NotFound();
            }

            return View(claseTipoCambio);
        }

        // POST: ClaseTipoCambio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var claseTipoCambio = await _context.ClaseTipoCambio.FindAsync(id);
            if (claseTipoCambio != null)
            {
                _context.ClaseTipoCambio.Remove(claseTipoCambio);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ClaseTipoCambio/ExportToExcel
        public async Task<IActionResult> ExportToExcel()
        {
            var clasesTipoCambio = await _context.ClaseTipoCambio
                .OrderBy(c => c.IdClaseTipCam)
                .ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Clases Tipo de Cambio");

                // Headers
                worksheet.Cells[1, 1].Value = "ID Clase Tipo Cambio";
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
                foreach (var claseTipoCambio in clasesTipoCambio)
                {
                    worksheet.Cells[row, 1].Value = claseTipoCambio.IdClaseTipCam;
                    worksheet.Cells[row, 2].Value = claseTipoCambio.Descripcion ?? "";
                    row++;
                }

                // Manually set column widths (AutoFit has issues on macOS)
                worksheet.Column(1).Width = 25; // ID
                worksheet.Column(2).Width = 30; // Descripción

                var fileName = "ClasesTipoCambio_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
                return File(package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
        }

        private bool ClaseTipoCambioExists(short id)
        {
            return _context.ClaseTipoCambio.Any(e => e.IdClaseTipCam == id);
        }
    }
}
