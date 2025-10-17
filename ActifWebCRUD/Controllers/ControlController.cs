using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class ControlController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public ControlController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Control
        public async Task<IActionResult> Index()
        {
            var controles = await _context.Control.ToListAsync();
            return View(controles);
        }

        // GET: Control/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var control = await _context.Control
                .FirstOrDefaultAsync(m => m.IdControl == id);
            if (control == null)
            {
                return NotFound();
            }

            return View(control);
        }

        // GET: Control/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Control/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdControl,Titulo,Contenido,FlgUsar,Tipo,Longitud,Decimales")] Control control)
        {
            if (ModelState.IsValid)
            {
                _context.Add(control);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(control);
        }

        // GET: Control/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var control = await _context.Control.FindAsync(id);
            if (control == null)
            {
                return NotFound();
            }
            return View(control);
        }

        // POST: Control/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("IdControl,Titulo,Contenido,FlgUsar,Tipo,Longitud,Decimales")] Control control)
        {
            if (id != control.IdControl)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the original rv value
                    var original = await _context.Control.AsNoTracking().FirstOrDefaultAsync(c => c.IdControl == id);
                    if (original != null)
                    {
                        control.Rv = original.Rv;
                    }

                    _context.Update(control);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ControlExists(control.IdControl))
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
            return View(control);
        }

        // GET: Control/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var control = await _context.Control
                .FirstOrDefaultAsync(m => m.IdControl == id);
            if (control == null)
            {
                return NotFound();
            }

            return View(control);
        }

        // POST: Control/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var control = await _context.Control.FindAsync(id);
            if (control != null)
            {
                _context.Control.Remove(control);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var controles = await _context.Control.ToListAsync();

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Control");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID Control";
                worksheet.Cells[1, 2].Value = "Titulo";
                worksheet.Cells[1, 3].Value = "Contenido";
                worksheet.Cells[1, 4].Value = "Flag Usar";
                worksheet.Cells[1, 5].Value = "Tipo";
                worksheet.Cells[1, 6].Value = "Longitud";
                worksheet.Cells[1, 7].Value = "Decimales";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 7])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var control in controles)
                {
                    worksheet.Cells[row, 1].Value = control.IdControl;
                    worksheet.Cells[row, 2].Value = control.Titulo;
                    worksheet.Cells[row, 3].Value = control.Contenido;
                    worksheet.Cells[row, 4].Value = control.FlgUsar;
                    worksheet.Cells[row, 5].Value = control.Tipo;
                    worksheet.Cells[row, 6].Value = control.Longitud;
                    worksheet.Cells[row, 7].Value = control.Decimales;
                    row++;
                }

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Return the Excel file
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Control_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
        }

        private bool ControlExists(short id)
        {
            return _context.Control.Any(e => e.IdControl == id);
        }
    }
}
