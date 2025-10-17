using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class GrupoEdificioDetalleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public GrupoEdificioDetalleController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: GrupoEdificioDetalle
        public async Task<IActionResult> Index()
        {
            var detalles = await _context.GrupoEdificioDetalle
                .Include(d => d.GrupoEdificio)
                .Include(d => d.Edificio)
                .ToListAsync();
            return View(detalles);
        }

        // GET: GrupoEdificioDetalle/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle = await _context.GrupoEdificioDetalle
                .Include(d => d.GrupoEdificio)
                .Include(d => d.Edificio)
                .FirstOrDefaultAsync(m => m.IdGE == id);

            if (detalle == null)
            {
                return NotFound();
            }

            return View(detalle);
        }

        // GET: GrupoEdificioDetalle/Create
        public IActionResult Create()
        {
            ViewData["IdGrupo"] = new SelectList(_context.GrupoEdificio, "IdGrupo", "Grupo");
            ViewData["IdEdificio"] = new SelectList(_context.Edificio, "IdEdificio", "IdEdificio");
            return View();
        }

        // POST: GrupoEdificioDetalle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGE,IdGrupo,IdEdificio")] GrupoEdificioDetalle detalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdGrupo"] = new SelectList(_context.GrupoEdificio, "IdGrupo", "Grupo", detalle.IdGrupo);
            ViewData["IdEdificio"] = new SelectList(_context.Edificio, "IdEdificio", "IdEdificio", detalle.IdEdificio);
            return View(detalle);
        }

        // GET: GrupoEdificioDetalle/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle = await _context.GrupoEdificioDetalle.FindAsync(id);
            if (detalle == null)
            {
                return NotFound();
            }

            ViewData["IdGrupo"] = new SelectList(_context.GrupoEdificio, "IdGrupo", "Grupo", detalle.IdGrupo);
            ViewData["IdEdificio"] = new SelectList(_context.Edificio, "IdEdificio", "IdEdificio", detalle.IdEdificio);
            return View(detalle);
        }

        // POST: GrupoEdificioDetalle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGE,IdGrupo,IdEdificio")] GrupoEdificioDetalle detalle)
        {
            if (id != detalle.IdGE)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GrupoEdificioDetalleExists(detalle.IdGE))
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

            ViewData["IdGrupo"] = new SelectList(_context.GrupoEdificio, "IdGrupo", "Grupo", detalle.IdGrupo);
            ViewData["IdEdificio"] = new SelectList(_context.Edificio, "IdEdificio", "IdEdificio", detalle.IdEdificio);
            return View(detalle);
        }

        // GET: GrupoEdificioDetalle/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle = await _context.GrupoEdificioDetalle
                .Include(d => d.GrupoEdificio)
                .Include(d => d.Edificio)
                .FirstOrDefaultAsync(m => m.IdGE == id);

            if (detalle == null)
            {
                return NotFound();
            }

            return View(detalle);
        }

        // POST: GrupoEdificioDetalle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalle = await _context.GrupoEdificioDetalle.FindAsync(id);
            if (detalle != null)
            {
                _context.GrupoEdificioDetalle.Remove(detalle);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var detalles = await _context.GrupoEdificioDetalle
                .Include(d => d.GrupoEdificio)
                .Include(d => d.Edificio)
                .ToListAsync();

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Grupo Edificio Detalle");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Grupo";
                worksheet.Cells[1, 3].Value = "Edificio";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 3])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var detalle in detalles)
                {
                    worksheet.Cells[row, 1].Value = detalle.IdGE;
                    worksheet.Cells[row, 2].Value = detalle.GrupoEdificio?.Grupo ?? "";
                    worksheet.Cells[row, 3].Value = detalle.IdEdificio;
                    row++;
                }

                // Set column widths manually (AutoFitColumns not supported on non-Windows)
                worksheet.Column(1).Width = 10;
                worksheet.Column(2).Width = 25;
                worksheet.Column(3).Width = 15;

                // Return the Excel file
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"GrupoEdificioDetalle_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
        }

        private bool GrupoEdificioDetalleExists(int id)
        {
            return _context.GrupoEdificioDetalle.Any(e => e.IdGE == id);
        }
    }
}
