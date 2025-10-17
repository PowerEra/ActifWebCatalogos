using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class GrupoSimulacionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public GrupoSimulacionController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: GrupoSimulacion
        public async Task<IActionResult> Index()
        {
            var gruposSimulacion = await _context.GrupoSimulacion.ToListAsync();
            return View(gruposSimulacion);
        }

        // GET: GrupoSimulacion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grupoSimulacion = await _context.GrupoSimulacion
                .FirstOrDefaultAsync(m => m.IdGrupoSimulacion == id);
            if (grupoSimulacion == null)
            {
                return NotFound();
            }

            return View(grupoSimulacion);
        }

        // GET: GrupoSimulacion/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GrupoSimulacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Descripcion,Nota")] GrupoSimulacion grupoSimulacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grupoSimulacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(grupoSimulacion);
        }

        // GET: GrupoSimulacion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grupoSimulacion = await _context.GrupoSimulacion.FindAsync(id);
            if (grupoSimulacion == null)
            {
                return NotFound();
            }
            return View(grupoSimulacion);
        }

        // POST: GrupoSimulacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGrupoSimulacion,Descripcion,Nota")] GrupoSimulacion grupoSimulacion)
        {
            if (id != grupoSimulacion.IdGrupoSimulacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the original rv value
                    var original = await _context.GrupoSimulacion.AsNoTracking().FirstOrDefaultAsync(g => g.IdGrupoSimulacion == id);
                    if (original != null)
                    {
                        grupoSimulacion.Rv = original.Rv;
                    }

                    _context.Update(grupoSimulacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GrupoSimulacionExists(grupoSimulacion.IdGrupoSimulacion))
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
            return View(grupoSimulacion);
        }

        // GET: GrupoSimulacion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grupoSimulacion = await _context.GrupoSimulacion
                .FirstOrDefaultAsync(m => m.IdGrupoSimulacion == id);
            if (grupoSimulacion == null)
            {
                return NotFound();
            }

            return View(grupoSimulacion);
        }

        // POST: GrupoSimulacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grupoSimulacion = await _context.GrupoSimulacion.FindAsync(id);
            if (grupoSimulacion != null)
            {
                _context.GrupoSimulacion.Remove(grupoSimulacion);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var gruposSimulacion = await _context.GrupoSimulacion.ToListAsync();

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Grupos Simulacion");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID Grupo Simulacion";
                worksheet.Cells[1, 2].Value = "Descripcion";
                worksheet.Cells[1, 3].Value = "Nota";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 3])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var grupo in gruposSimulacion)
                {
                    worksheet.Cells[row, 1].Value = grupo.IdGrupoSimulacion;
                    worksheet.Cells[row, 2].Value = grupo.Descripcion;
                    worksheet.Cells[row, 3].Value = grupo.Nota;
                    row++;
                }

                // Set column widths manually (AutoFitColumns requires System.Drawing on macOS)
                worksheet.Column(1).Width = 20;
                worksheet.Column(2).Width = 30;
                worksheet.Column(3).Width = 50;

                // Return the Excel file
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"GruposSimulacion_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
        }

        private bool GrupoSimulacionExists(int id)
        {
            return _context.GrupoSimulacion.Any(e => e.IdGrupoSimulacion == id);
        }
    }
}
