using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class GrupoEdificioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public GrupoEdificioController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: GrupoEdificio
        public async Task<IActionResult> Index()
        {
            var gruposEdificio = await _context.GrupoEdificio.ToListAsync();
            return View(gruposEdificio);
        }

        // GET: GrupoEdificio/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grupoEdificio = await _context.GrupoEdificio
                .FirstOrDefaultAsync(m => m.IdGrupo == id);
            if (grupoEdificio == null)
            {
                return NotFound();
            }

            return View(grupoEdificio);
        }

        // GET: GrupoEdificio/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GrupoEdificio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGrupo,Grupo")] GrupoEdificio grupoEdificio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grupoEdificio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(grupoEdificio);
        }

        // GET: GrupoEdificio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grupoEdificio = await _context.GrupoEdificio.FindAsync(id);
            if (grupoEdificio == null)
            {
                return NotFound();
            }
            return View(grupoEdificio);
        }

        // POST: GrupoEdificio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGrupo,Grupo")] GrupoEdificio grupoEdificio)
        {
            if (id != grupoEdificio.IdGrupo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grupoEdificio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GrupoEdificioExists(grupoEdificio.IdGrupo))
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
            return View(grupoEdificio);
        }

        // GET: GrupoEdificio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grupoEdificio = await _context.GrupoEdificio
                .FirstOrDefaultAsync(m => m.IdGrupo == id);
            if (grupoEdificio == null)
            {
                return NotFound();
            }

            return View(grupoEdificio);
        }

        // POST: GrupoEdificio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grupoEdificio = await _context.GrupoEdificio.FindAsync(id);
            if (grupoEdificio != null)
            {
                _context.GrupoEdificio.Remove(grupoEdificio);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var gruposEdificio = await _context.GrupoEdificio.ToListAsync();

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Grupos Edificio");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID Grupo";
                worksheet.Cells[1, 2].Value = "Grupo";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 2])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var grupo in gruposEdificio)
                {
                    worksheet.Cells[row, 1].Value = grupo.IdGrupo;
                    worksheet.Cells[row, 2].Value = grupo.Grupo;
                    row++;
                }

                // Set column widths manually (AutoFitColumns not supported on non-Windows)
                worksheet.Column(1).Width = 15;
                worksheet.Column(2).Width = 25;

                // Return the Excel file
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"GrupoEdificio_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
        }

        private bool GrupoEdificioExists(int id)
        {
            return _context.GrupoEdificio.Any(e => e.IdGrupo == id);
        }
    }
}
