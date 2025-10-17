using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class EstadoActivoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public EstadoActivoController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: EstadoActivo
        public async Task<IActionResult> Index()
        {
            var estadosActivo = await _context.EstadoActivo.ToListAsync();
            return View(estadosActivo);
        }

        // GET: EstadoActivo/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoActivo = await _context.EstadoActivo
                .FirstOrDefaultAsync(m => m.IdEstadoActivo == id);
            if (estadoActivo == null)
            {
                return NotFound();
            }

            return View(estadoActivo);
        }

        // GET: EstadoActivo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadoActivo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Descripcion")] EstadoActivo estadoActivo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadoActivo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadoActivo);
        }

        // GET: EstadoActivo/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoActivo = await _context.EstadoActivo.FindAsync(id);
            if (estadoActivo == null)
            {
                return NotFound();
            }
            return View(estadoActivo);
        }

        // POST: EstadoActivo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("IdEstadoActivo,Descripcion")] EstadoActivo estadoActivo)
        {
            if (id != estadoActivo.IdEstadoActivo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the original rv value
                    var original = await _context.EstadoActivo.AsNoTracking().FirstOrDefaultAsync(e => e.IdEstadoActivo == id);
                    if (original != null)
                    {
                        estadoActivo.Rv = original.Rv;
                    }

                    _context.Update(estadoActivo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoActivoExists(estadoActivo.IdEstadoActivo))
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
            return View(estadoActivo);
        }

        // GET: EstadoActivo/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoActivo = await _context.EstadoActivo
                .FirstOrDefaultAsync(m => m.IdEstadoActivo == id);
            if (estadoActivo == null)
            {
                return NotFound();
            }

            return View(estadoActivo);
        }

        // POST: EstadoActivo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var estadoActivo = await _context.EstadoActivo.FindAsync(id);
            if (estadoActivo != null)
            {
                _context.EstadoActivo.Remove(estadoActivo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel
        public async Task<IActionResult> ExportToExcel()
        {
            var estadosActivo = await _context.EstadoActivo.ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("EstadosActivo");

                // Headers (row 1)
                worksheet.Cells[1, 1].Value = "ID Estado Activo";
                worksheet.Cells[1, 2].Value = "Descripcion";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 2])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Data (starting row 2)
                int row = 2;
                foreach (var estado in estadosActivo)
                {
                    worksheet.Cells[row, 1].Value = estado.IdEstadoActivo;
                    worksheet.Cells[row, 2].Value = estado.Descripcion;
                    row++;
                }

                // Manual column widths (AutoFit doesn't work on macOS)
                worksheet.Column(1).Width = 20;
                worksheet.Column(2).Width = 30;

                return File(package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"EstadosActivo_{DateTime.Now:yyyyMMdd}.xlsx");
            }
        }

        private bool EstadoActivoExists(short id)
        {
            return _context.EstadoActivo.Any(e => e.IdEstadoActivo == id);
        }
    }
}
