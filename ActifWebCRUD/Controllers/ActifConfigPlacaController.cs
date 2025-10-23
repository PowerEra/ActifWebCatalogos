using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using ActifWebCRUD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class ActifConfigPlacaController : Controller
    {
        private readonly CookieAuthenticationService _authService;
        private readonly ApplicationDbContext _context;

        public ActifConfigPlacaController(CookieAuthenticationService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        // GET: ActifConfigPlaca
        public async Task<IActionResult> Index()
        {
            var user = _authService.GetUserFromCookie(HttpContext);

            if (user == null)
            {
                return View(new List<ActifConfigPlaca>());
            }

            var actifConfigPlacas = await _context.ActifConfigPlaca
                .Where(a => a.IdCompania == user.IdCompania)
                .ToListAsync();

            // Load Compania manually for each item
            foreach (var item in actifConfigPlacas)
            {
                if (item.IdCompania != null)
                {
                    var compania = await _context.Compania.FindAsync((short)item.IdCompania.Value);
                    item.Compania = compania;
                }
            }

            return View(actifConfigPlacas);
        }

        // GET: ActifConfigPlaca/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actifConfigPlaca = await _context.ActifConfigPlaca
                .FirstOrDefaultAsync(m => m.IdConfigPlaca == id);

            if (actifConfigPlaca == null)
            {
                return NotFound();
            }

            // Load Compania manually for display
            if (actifConfigPlaca.IdCompania != null)
            {
                var compania = await _context.Compania.FindAsync((short)actifConfigPlaca.IdCompania.Value);
                actifConfigPlaca.Compania = compania;
            }

            return View(actifConfigPlaca);
        }

        // GET: ActifConfigPlaca/Create
        public IActionResult Create()
        {
            ViewBag.IdCompania = new SelectList(_context.Compania, "IdCompania", "Nombre");
            return View();
        }

        // POST: ActifConfigPlaca/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdConfigPlaca,IdCompania,Prefijo,MinDigitos,MaxDigitos")] ActifConfigPlaca actifConfigPlaca)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actifConfigPlaca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.IdCompania = new SelectList(_context.Compania, "IdCompania", "Nombre", actifConfigPlaca.IdCompania);
            return View(actifConfigPlaca);
        }

        // GET: ActifConfigPlaca/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actifConfigPlaca = await _context.ActifConfigPlaca.FindAsync(id);
            if (actifConfigPlaca == null)
            {
                return NotFound();
            }
            ViewBag.IdCompania = new SelectList(_context.Compania, "IdCompania", "Nombre", actifConfigPlaca.IdCompania);
            return View(actifConfigPlaca);
        }

        // POST: ActifConfigPlaca/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdConfigPlaca,IdCompania,Prefijo,MinDigitos,MaxDigitos")] ActifConfigPlaca actifConfigPlaca)
        {
            if (id != actifConfigPlaca.IdConfigPlaca)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the original rv value
                    var original = await _context.ActifConfigPlaca.AsNoTracking().FirstOrDefaultAsync(c => c.IdConfigPlaca == id);
                    if (original != null)
                    {
                        actifConfigPlaca.Rv = original.Rv;
                    }

                    _context.Update(actifConfigPlaca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActifConfigPlacaExists(actifConfigPlaca.IdConfigPlaca))
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
            ViewBag.IdCompania = new SelectList(_context.Compania, "IdCompania", "Nombre", actifConfigPlaca.IdCompania);
            return View(actifConfigPlaca);
        }

        // GET: ActifConfigPlaca/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actifConfigPlaca = await _context.ActifConfigPlaca
                .FirstOrDefaultAsync(m => m.IdConfigPlaca == id);

            if (actifConfigPlaca == null)
            {
                return NotFound();
            }

            // Load Compania manually for display
            if (actifConfigPlaca.IdCompania != null)
            {
                var compania = await _context.Compania.FindAsync((short)actifConfigPlaca.IdCompania.Value);
                actifConfigPlaca.Compania = compania;
            }

            return View(actifConfigPlaca);
        }

        // POST: ActifConfigPlaca/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actifConfigPlaca = await _context.ActifConfigPlaca.FindAsync(id);
            if (actifConfigPlaca != null)
            {
                _context.ActifConfigPlaca.Remove(actifConfigPlaca);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var actifConfigPlacas = await _context.ActifConfigPlaca.ToListAsync();

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Configuracion Placas");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID Config Placa";
                worksheet.Cells[1, 2].Value = "ID Compania";
                worksheet.Cells[1, 3].Value = "Prefijo";
                worksheet.Cells[1, 4].Value = "Min Digitos";
                worksheet.Cells[1, 5].Value = "Max Digitos";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var actifConfigPlaca in actifConfigPlacas)
                {
                    worksheet.Cells[row, 1].Value = actifConfigPlaca.IdConfigPlaca;
                    worksheet.Cells[row, 2].Value = actifConfigPlaca.IdCompania;
                    worksheet.Cells[row, 3].Value = actifConfigPlaca.Prefijo;
                    worksheet.Cells[row, 4].Value = actifConfigPlaca.MinDigitos;
                    worksheet.Cells[row, 5].Value = actifConfigPlaca.MaxDigitos;
                    row++;
                }

                // Set column widths manually (AutoFitColumns requires System.Drawing which is not cross-platform)
                worksheet.Column(1).Width = 20;
                worksheet.Column(2).Width = 15;
                worksheet.Column(3).Width = 20;
                worksheet.Column(4).Width = 15;
                worksheet.Column(5).Width = 15;

                // Return the Excel file
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"ActifConfigPlaca_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
        }

        private bool ActifConfigPlacaExists(int id)
        {
            return _context.ActifConfigPlaca.Any(e => e.IdConfigPlaca == id);
        }
    }
}
