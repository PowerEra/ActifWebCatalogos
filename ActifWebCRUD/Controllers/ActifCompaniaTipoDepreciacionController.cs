using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using ActifWebCRUD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class ActifCompaniaTipoDepreciacionController : Controller
    {
        private readonly CookieAuthenticationService _authService;
        private readonly ApplicationDbContext _context;

        public ActifCompaniaTipoDepreciacionController(CookieAuthenticationService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        // GET: ActifCompaniaTipoDepreciacion
        public async Task<IActionResult> Index()
        {
            var user = _authService.GetUserFromCookie(HttpContext);

            if (user == null)
            {
                return View(new List<ActifCompaniaTipoDepreciacion>());
            }

            var items = await _context.ActifCompaniaTipoDepreciacion
                .Include(a => a.Compania)
                .Include(a => a.TipoDepreciacion)
                .Where(a => a.IdCompania == user.IdCompania)
                .ToListAsync();
            return View(items);
        }

        // GET: ActifCompaniaTipoDepreciacion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.ActifCompaniaTipoDepreciacion
                .Include(a => a.Compania)
                .Include(a => a.TipoDepreciacion)
                .FirstOrDefaultAsync(m => m.IdCompaniaDepreciacion == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: ActifCompaniaTipoDepreciacion/Create
        public IActionResult Create()
        {
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre");
            ViewData["IdTipoDep"] = new SelectList(_context.TipoDepreciacion, "IdTipoDep", "Descripcion");
            return View();
        }

        // POST: ActifCompaniaTipoDepreciacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCompaniaDepreciacion,IdCompania,IdTipoDep")] ActifCompaniaTipoDepreciacion item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre", item.IdCompania);
            ViewData["IdTipoDep"] = new SelectList(_context.TipoDepreciacion, "IdTipoDep", "Descripcion", item.IdTipoDep);
            return View(item);
        }

        // GET: ActifCompaniaTipoDepreciacion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.ActifCompaniaTipoDepreciacion.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre", item.IdCompania);
            ViewData["IdTipoDep"] = new SelectList(_context.TipoDepreciacion, "IdTipoDep", "Descripcion", item.IdTipoDep);
            return View(item);
        }

        // POST: ActifCompaniaTipoDepreciacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCompaniaDepreciacion,IdCompania,IdTipoDep")] ActifCompaniaTipoDepreciacion item)
        {
            if (id != item.IdCompaniaDepreciacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActifCompaniaTipoDepreciacionExists(item.IdCompaniaDepreciacion))
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
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre", item.IdCompania);
            ViewData["IdTipoDep"] = new SelectList(_context.TipoDepreciacion, "IdTipoDep", "Descripcion", item.IdTipoDep);
            return View(item);
        }

        // GET: ActifCompaniaTipoDepreciacion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.ActifCompaniaTipoDepreciacion
                .Include(a => a.Compania)
                .Include(a => a.TipoDepreciacion)
                .FirstOrDefaultAsync(m => m.IdCompaniaDepreciacion == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: ActifCompaniaTipoDepreciacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.ActifCompaniaTipoDepreciacion.FindAsync(id);
            if (item != null)
            {
                _context.ActifCompaniaTipoDepreciacion.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var items = await _context.ActifCompaniaTipoDepreciacion
                .Include(a => a.Compania)
                .Include(a => a.TipoDepreciacion)
                .ToListAsync();

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("CompaniaTipoDepreciacion");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID Compania Depreciacion";
                worksheet.Cells[1, 2].Value = "ID Compania";
                worksheet.Cells[1, 3].Value = "Nombre Compania";
                worksheet.Cells[1, 4].Value = "ID Tipo Depreciacion";
                worksheet.Cells[1, 5].Value = "Descripcion Tipo Depreciacion";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var item in items)
                {
                    worksheet.Cells[row, 1].Value = item.IdCompaniaDepreciacion;
                    worksheet.Cells[row, 2].Value = item.IdCompania;
                    worksheet.Cells[row, 3].Value = item.Compania?.Nombre;
                    worksheet.Cells[row, 4].Value = item.IdTipoDep;
                    worksheet.Cells[row, 5].Value = item.TipoDepreciacion?.Descripcion;
                    row++;
                }

                // Set column widths manually (AutoFitColumns not supported on non-Windows)
                for (int col = 1; col <= 5; col++)
                {
                    worksheet.Column(col).Width = 30;
                }

                // Return the Excel file
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"ActifCompaniaTipoDepreciacion_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
        }

        private bool ActifCompaniaTipoDepreciacionExists(int id)
        {
            return _context.ActifCompaniaTipoDepreciacion.Any(e => e.IdCompaniaDepreciacion == id);
        }
    }
}
