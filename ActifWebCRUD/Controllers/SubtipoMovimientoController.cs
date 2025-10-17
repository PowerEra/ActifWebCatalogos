using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class SubtipoMovimientoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public SubtipoMovimientoController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: SubtipoMovimiento
        public async Task<IActionResult> Index()
        {
            var subtiposMovimiento = await _context.SubtipoMovimiento
                .Include(s => s.TipoMovimiento)
                .ToListAsync();
            return View(subtiposMovimiento);
        }

        // GET: SubtipoMovimiento/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtipoMovimiento = await _context.SubtipoMovimiento
                .Include(s => s.TipoMovimiento)
                .FirstOrDefaultAsync(m => m.IdSubtipoMov == id);
            if (subtipoMovimiento == null)
            {
                return NotFound();
            }

            return View(subtipoMovimiento);
        }

        // GET: SubtipoMovimiento/Create
        public IActionResult Create()
        {
            PopulateTipoMovimientoDropdown();
            return View();
        }

        // POST: SubtipoMovimiento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoMov,Descripcion,AfectacionInterfaz,FlgVisualizar,FlgGrabahist")] SubtipoMovimiento subtipoMovimiento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subtipoMovimiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateTipoMovimientoDropdown(subtipoMovimiento.IdTipoMov);
            return View(subtipoMovimiento);
        }

        // GET: SubtipoMovimiento/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtipoMovimiento = await _context.SubtipoMovimiento.FindAsync(id);
            if (subtipoMovimiento == null)
            {
                return NotFound();
            }
            PopulateTipoMovimientoDropdown(subtipoMovimiento.IdTipoMov);
            return View(subtipoMovimiento);
        }

        // POST: SubtipoMovimiento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("IdSubtipoMov,IdTipoMov,Descripcion,AfectacionInterfaz,FlgVisualizar,FlgGrabahist")] SubtipoMovimiento subtipoMovimiento)
        {
            if (id != subtipoMovimiento.IdSubtipoMov)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subtipoMovimiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubtipoMovimientoExists(subtipoMovimiento.IdSubtipoMov))
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
            PopulateTipoMovimientoDropdown(subtipoMovimiento.IdTipoMov);
            return View(subtipoMovimiento);
        }

        // GET: SubtipoMovimiento/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtipoMovimiento = await _context.SubtipoMovimiento
                .Include(s => s.TipoMovimiento)
                .FirstOrDefaultAsync(m => m.IdSubtipoMov == id);
            if (subtipoMovimiento == null)
            {
                return NotFound();
            }

            return View(subtipoMovimiento);
        }

        // POST: SubtipoMovimiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var subtipoMovimiento = await _context.SubtipoMovimiento.FindAsync(id);
            if (subtipoMovimiento != null)
            {
                _context.SubtipoMovimiento.Remove(subtipoMovimiento);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel
        public async Task<IActionResult> ExportToExcel()
        {
            var subtiposMovimiento = await _context.SubtipoMovimiento
                .Include(s => s.TipoMovimiento)
                .ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("SubtiposMovimiento");

                // Headers (row 1)
                worksheet.Cells[1, 1].Value = "ID Subtipo Movimiento";
                worksheet.Cells[1, 2].Value = "Tipo Movimiento";
                worksheet.Cells[1, 3].Value = "Descripcion";
                worksheet.Cells[1, 4].Value = "Afectacion Interfaz";
                worksheet.Cells[1, 5].Value = "Flag Visualizar";
                worksheet.Cells[1, 6].Value = "Flag Graba Historial";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Data (starting row 2)
                int row = 2;
                foreach (var subtipo in subtiposMovimiento)
                {
                    worksheet.Cells[row, 1].Value = subtipo.IdSubtipoMov;
                    worksheet.Cells[row, 2].Value = subtipo.TipoMovimiento?.Descripcion;
                    worksheet.Cells[row, 3].Value = subtipo.Descripcion;
                    worksheet.Cells[row, 4].Value = subtipo.AfectacionInterfaz;
                    worksheet.Cells[row, 5].Value = subtipo.FlgVisualizar;
                    worksheet.Cells[row, 6].Value = subtipo.FlgGrabahist;
                    row++;
                }

                // Manual column widths (AutoFit doesn't work on macOS)
                worksheet.Column(1).Width = 25;
                worksheet.Column(2).Width = 30;
                worksheet.Column(3).Width = 30;
                worksheet.Column(4).Width = 25;
                worksheet.Column(5).Width = 20;
                worksheet.Column(6).Width = 25;

                return File(package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"SubtiposMovimiento_{DateTime.Now:yyyyMMdd}.xlsx");
            }
        }

        private void PopulateTipoMovimientoDropdown(object? selectedValue = null)
        {
            var tiposMovimiento = _context.TipoMovimiento
                .OrderBy(t => t.Descripcion)
                .Select(t => new SelectListItem
                {
                    Value = t.IdTipoMov.ToString(),
                    Text = t.Descripcion
                })
                .ToList();

            tiposMovimiento.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione un Tipo de Movimiento --" });

            ViewBag.IdTipoMov = new SelectList(tiposMovimiento, "Value", "Text", selectedValue);
        }

        private bool SubtipoMovimientoExists(short id)
        {
            return _context.SubtipoMovimiento.Any(e => e.IdSubtipoMov == id);
        }
    }
}
