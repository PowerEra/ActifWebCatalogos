using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class SubtipoActivoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public SubtipoActivoController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: SubtipoActivo
        public async Task<IActionResult> Index()
        {
            var subtiposActivo = await _context.SubtipoActivo
                .Include(s => s.TipoActivo)
                .ToListAsync();
            return View(subtiposActivo);
        }

        // GET: SubtipoActivo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtipoActivo = await _context.SubtipoActivo
                .Include(s => s.TipoActivo)
                .FirstOrDefaultAsync(m => m.IdSubtipoActivo == id);
            if (subtipoActivo == null)
            {
                return NotFound();
            }

            return View(subtipoActivo);
        }

        // GET: SubtipoActivo/Create
        public IActionResult Create()
        {
            PopulateTipoActivoDropdown();
            return View();
        }

        // POST: SubtipoActivo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSubtipoActivo,IdTipoActivo,Descripcion,Actualizar,Codigo")] SubtipoActivo subtipoActivo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subtipoActivo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateTipoActivoDropdown(subtipoActivo.IdTipoActivo);
            return View(subtipoActivo);
        }

        // GET: SubtipoActivo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtipoActivo = await _context.SubtipoActivo.FindAsync(id);
            if (subtipoActivo == null)
            {
                return NotFound();
            }
            PopulateTipoActivoDropdown(subtipoActivo.IdTipoActivo);
            return View(subtipoActivo);
        }

        // POST: SubtipoActivo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSubtipoActivo,IdTipoActivo,Descripcion,Actualizar,Codigo")] SubtipoActivo subtipoActivo)
        {
            if (id != subtipoActivo.IdSubtipoActivo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the original rv value
                    var original = await _context.SubtipoActivo.AsNoTracking().FirstOrDefaultAsync(s => s.IdSubtipoActivo == id);
                    if (original != null)
                    {
                        subtipoActivo.Rv = original.Rv;
                    }

                    _context.Update(subtipoActivo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubtipoActivoExists(subtipoActivo.IdSubtipoActivo))
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
            PopulateTipoActivoDropdown(subtipoActivo.IdTipoActivo);
            return View(subtipoActivo);
        }

        // GET: SubtipoActivo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtipoActivo = await _context.SubtipoActivo
                .Include(s => s.TipoActivo)
                .FirstOrDefaultAsync(m => m.IdSubtipoActivo == id);
            if (subtipoActivo == null)
            {
                return NotFound();
            }

            return View(subtipoActivo);
        }

        // POST: SubtipoActivo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subtipoActivo = await _context.SubtipoActivo.FindAsync(id);
            if (subtipoActivo != null)
            {
                _context.SubtipoActivo.Remove(subtipoActivo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel
        public async Task<IActionResult> ExportToExcel()
        {
            var subtiposActivo = await _context.SubtipoActivo
                .Include(s => s.TipoActivo)
                .ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("SubtiposActivo");

                // Headers (row 1)
                worksheet.Cells[1, 1].Value = "ID Subtipo Activo";
                worksheet.Cells[1, 2].Value = "ID Tipo Activo";
                worksheet.Cells[1, 3].Value = "Tipo Activo";
                worksheet.Cells[1, 4].Value = "Descripcion";
                worksheet.Cells[1, 5].Value = "Actualizar";
                worksheet.Cells[1, 6].Value = "Codigo";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Data (starting row 2)
                int row = 2;
                foreach (var subtipo in subtiposActivo)
                {
                    worksheet.Cells[row, 1].Value = subtipo.IdSubtipoActivo;
                    worksheet.Cells[row, 2].Value = subtipo.IdTipoActivo;
                    worksheet.Cells[row, 3].Value = subtipo.TipoActivo?.Descripcion;
                    worksheet.Cells[row, 4].Value = subtipo.Descripcion;
                    worksheet.Cells[row, 5].Value = subtipo.Actualizar;
                    worksheet.Cells[row, 6].Value = subtipo.Codigo;
                    row++;
                }

                // Manual column widths (AutoFit doesn't work on macOS)
                worksheet.Column(1).Width = 20;
                worksheet.Column(2).Width = 20;
                worksheet.Column(3).Width = 35;
                worksheet.Column(4).Width = 35;
                worksheet.Column(5).Width = 15;
                worksheet.Column(6).Width = 15;

                return File(package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"SubtiposActivo_{DateTime.Now:yyyyMMdd}.xlsx");
            }
        }

        private void PopulateTipoActivoDropdown(object? selectedValue = null)
        {
            var tiposActivo = _context.TipoActivo
                .OrderBy(t => t.Descripcion)
                .Select(t => new SelectListItem
                {
                    Value = t.IdTipoActivo.ToString(),
                    Text = t.Descripcion
                })
                .ToList();

            tiposActivo.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione un Tipo de Activo --" });

            ViewBag.IdTipoActivo = new SelectList(tiposActivo, "Value", "Text", selectedValue);
        }

        private bool SubtipoActivoExists(int id)
        {
            return _context.SubtipoActivo.Any(e => e.IdSubtipoActivo == id);
        }
    }
}
