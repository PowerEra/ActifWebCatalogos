using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class TipoActivoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public TipoActivoController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: TipoActivo
        public async Task<IActionResult> Index()
        {
            var tiposActivo = await _context.TipoActivo
                .Include(t => t.Compania)
                .ToListAsync();
            return View(tiposActivo);
        }

        // GET: TipoActivo/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoActivo = await _context.TipoActivo
                .Include(t => t.Compania)
                .FirstOrDefaultAsync(m => m.IdTipoActivo == id);
            if (tipoActivo == null)
            {
                return NotFound();
            }

            return View(tipoActivo);
        }

        // GET: TipoActivo/Create
        public IActionResult Create()
        {
            PopulateCompaniaDropdown();
            return View();
        }

        // POST: TipoActivo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoActivo,Descripcion,IdCompania")] TipoActivo tipoActivo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoActivo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateCompaniaDropdown(tipoActivo.IdCompania);
            return View(tipoActivo);
        }

        // GET: TipoActivo/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoActivo = await _context.TipoActivo.FindAsync(id);
            if (tipoActivo == null)
            {
                return NotFound();
            }
            PopulateCompaniaDropdown(tipoActivo.IdCompania);
            return View(tipoActivo);
        }

        // POST: TipoActivo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("IdTipoActivo,Descripcion,IdCompania")] TipoActivo tipoActivo)
        {
            if (id != tipoActivo.IdTipoActivo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the original rv value
                    var original = await _context.TipoActivo.AsNoTracking().FirstOrDefaultAsync(t => t.IdTipoActivo == id);
                    if (original != null)
                    {
                        tipoActivo.Rv = original.Rv;
                    }

                    _context.Update(tipoActivo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoActivoExists(tipoActivo.IdTipoActivo))
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
            PopulateCompaniaDropdown(tipoActivo.IdCompania);
            return View(tipoActivo);
        }

        // GET: TipoActivo/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoActivo = await _context.TipoActivo
                .Include(t => t.Compania)
                .FirstOrDefaultAsync(m => m.IdTipoActivo == id);
            if (tipoActivo == null)
            {
                return NotFound();
            }

            return View(tipoActivo);
        }

        // POST: TipoActivo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var tipoActivo = await _context.TipoActivo.FindAsync(id);
            if (tipoActivo != null)
            {
                _context.TipoActivo.Remove(tipoActivo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel
        public async Task<IActionResult> ExportToExcel()
        {
            var tiposActivo = await _context.TipoActivo
                .Include(t => t.Compania)
                .ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("TiposActivo");

                // Headers (row 1)
                worksheet.Cells[1, 1].Value = "ID Tipo Activo";
                worksheet.Cells[1, 2].Value = "Descripcion";
                worksheet.Cells[1, 3].Value = "Compania";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 3])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Data (starting row 2)
                int row = 2;
                foreach (var tipo in tiposActivo)
                {
                    worksheet.Cells[row, 1].Value = tipo.IdTipoActivo;
                    worksheet.Cells[row, 2].Value = tipo.Descripcion;
                    worksheet.Cells[row, 3].Value = tipo.Compania?.Nombre;
                    row++;
                }

                // Manual column widths (AutoFit doesn't work on macOS)
                worksheet.Column(1).Width = 20;
                worksheet.Column(2).Width = 40;
                worksheet.Column(3).Width = 30;

                return File(package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"TiposActivo_{DateTime.Now:yyyyMMdd}.xlsx");
            }
        }

        private void PopulateCompaniaDropdown(object? selectedValue = null)
        {
            var companias = _context.Compania
                .OrderBy(c => c.Nombre)
                .Select(c => new SelectListItem
                {
                    Value = c.IdCompania.ToString(),
                    Text = c.Nombre
                })
                .ToList();

            companias.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione una Compania --" });

            ViewBag.IdCompania = new SelectList(companias, "Value", "Text", selectedValue);
        }

        private bool TipoActivoExists(short id)
        {
            return _context.TipoActivo.Any(e => e.IdTipoActivo == id);
        }
    }
}
