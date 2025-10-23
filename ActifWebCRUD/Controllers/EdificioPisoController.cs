using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using ActifWebCRUD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class EdificioPisoController : Controller
    {
        private readonly CookieAuthenticationService _authService;
        private readonly ApplicationDbContext _context;

        public EdificioPisoController(CookieAuthenticationService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        // GET: EdificioPiso
        public async Task<IActionResult> Index()
        {
            var user = _authService.GetUserFromCookie(HttpContext);

            if (user == null)
            {
                return View(new List<EdificioPiso>());
            }

            var edificioPisos = await _context.EdificioPiso
                .Where(a => a.IdCompania == user.IdCompania)
                .ToListAsync();

            // Manually load navigation properties
            foreach (var ep in edificioPisos)
            {
                ep.Compania = await _context.Compania.FindAsync(ep.IdCompania);
                ep.Piso = await _context.Piso.FindAsync(ep.IdPiso);
            }

            return View(edificioPisos);
        }

        // GET: EdificioPiso/Details
        public async Task<IActionResult> Details(short? idCompania, short? idEdificio, short? idPiso)
        {
            if (idCompania == null || idEdificio == null || idPiso == null)
            {
                return NotFound();
            }

            var edificioPiso = await _context.EdificioPiso
                .FirstOrDefaultAsync(m => m.IdCompania == idCompania && m.IdEdificio == idEdificio && m.IdPiso == idPiso);

            if (edificioPiso == null)
            {
                return NotFound();
            }

            // Load navigation properties
            edificioPiso.Compania = await _context.Compania.FindAsync(edificioPiso.IdCompania);
            edificioPiso.Piso = await _context.Piso.FindAsync(edificioPiso.IdPiso);

            return View(edificioPiso);
        }

        // GET: EdificioPiso/Create
        public IActionResult Create()
        {
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre");
            // Get edificios and manually create list items
            var edificios = _context.Edificio.OrderBy(e => e.Descripcion).ToList();
            var edificioItems = edificios.Select(e => new SelectListItem
            {
                Value = e.IdEdificio.ToString(),
                Text = e.Descripcion ?? e.IdEdificio.ToString()
            }).ToList();
            ViewData["IdEdificio"] = edificioItems;
            ViewData["IdPiso"] = new SelectList(_context.Piso, "IdPiso", "Descripcion");
            return View();
        }

        // POST: EdificioPiso/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCompania,IdEdificio,IdPiso")] EdificioPiso edificioPiso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(edificioPiso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre", edificioPiso.IdCompania);
            var edificios = _context.Edificio.OrderBy(e => e.Descripcion).ToList();
            var edificioItems = edificios.Select(e => new SelectListItem
            {
                Value = e.IdEdificio.ToString(),
                Text = e.Descripcion ?? e.IdEdificio.ToString(),
                Selected = e.IdEdificio == edificioPiso.IdEdificio
            }).ToList();
            ViewData["IdEdificio"] = edificioItems;
            ViewData["IdPiso"] = new SelectList(_context.Piso, "IdPiso", "Descripcion", edificioPiso.IdPiso);
            return View(edificioPiso);
        }

        // GET: EdificioPiso/Edit
        public async Task<IActionResult> Edit(short? idCompania, short? idEdificio, short? idPiso)
        {
            if (idCompania == null || idEdificio == null || idPiso == null)
            {
                return NotFound();
            }

            var edificioPiso = await _context.EdificioPiso
                .FirstOrDefaultAsync(m => m.IdCompania == idCompania && m.IdEdificio == idEdificio && m.IdPiso == idPiso);

            if (edificioPiso == null)
            {
                return NotFound();
            }

            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre", edificioPiso.IdCompania);
            var edificios = _context.Edificio.OrderBy(e => e.Descripcion).ToList();
            var edificioItems = edificios.Select(e => new SelectListItem
            {
                Value = e.IdEdificio.ToString(),
                Text = e.Descripcion ?? e.IdEdificio.ToString(),
                Selected = e.IdEdificio == edificioPiso.IdEdificio
            }).ToList();
            ViewData["IdEdificio"] = edificioItems;
            ViewData["IdPiso"] = new SelectList(_context.Piso, "IdPiso", "Descripcion", edificioPiso.IdPiso);
            return View(edificioPiso);
        }

        // POST: EdificioPiso/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short idCompania, short idEdificio, short idPiso, [Bind("IdCompania,IdEdificio,IdPiso")] EdificioPiso edificioPiso)
        {
            if (idCompania != edificioPiso.IdCompania || idEdificio != edificioPiso.IdEdificio || idPiso != edificioPiso.IdPiso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(edificioPiso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EdificioPisoExists(edificioPiso.IdCompania, edificioPiso.IdEdificio, edificioPiso.IdPiso))
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
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre", edificioPiso.IdCompania);
            var edificios = _context.Edificio.OrderBy(e => e.Descripcion).ToList();
            var edificioItems = edificios.Select(e => new SelectListItem
            {
                Value = e.IdEdificio.ToString(),
                Text = e.Descripcion ?? e.IdEdificio.ToString(),
                Selected = e.IdEdificio == edificioPiso.IdEdificio
            }).ToList();
            ViewData["IdEdificio"] = edificioItems;
            ViewData["IdPiso"] = new SelectList(_context.Piso, "IdPiso", "Descripcion", edificioPiso.IdPiso);
            return View(edificioPiso);
        }

        // GET: EdificioPiso/Delete
        public async Task<IActionResult> Delete(short? idCompania, short? idEdificio, short? idPiso)
        {
            if (idCompania == null || idEdificio == null || idPiso == null)
            {
                return NotFound();
            }

            var edificioPiso = await _context.EdificioPiso
                .FirstOrDefaultAsync(m => m.IdCompania == idCompania && m.IdEdificio == idEdificio && m.IdPiso == idPiso);

            if (edificioPiso == null)
            {
                return NotFound();
            }

            // Load navigation properties
            edificioPiso.Compania = await _context.Compania.FindAsync(edificioPiso.IdCompania);
            edificioPiso.Piso = await _context.Piso.FindAsync(edificioPiso.IdPiso);

            return View(edificioPiso);
        }

        // POST: EdificioPiso/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short idCompania, short idEdificio, short idPiso)
        {
            var edificioPiso = await _context.EdificioPiso
                .FirstOrDefaultAsync(m => m.IdCompania == idCompania && m.IdEdificio == idEdificio && m.IdPiso == idPiso);

            if (edificioPiso != null)
            {
                _context.EdificioPiso.Remove(edificioPiso);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var edificioPisos = await _context.EdificioPiso.ToListAsync();

            // Pre-load all related data
            var companias = await _context.Compania.ToDictionaryAsync(c => c.IdCompania, c => c.Nombre);
            var edificios = await _context.Edificio.ToDictionaryAsync(e => e.IdEdificio, e => e.Descripcion);
            var pisos = await _context.Piso.ToDictionaryAsync(p => p.IdPiso, p => p.Descripcion);

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Edificio Piso");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID Compañía";
                worksheet.Cells[1, 2].Value = "Nombre Compañía";
                worksheet.Cells[1, 3].Value = "ID Edificio";
                worksheet.Cells[1, 4].Value = "Descripción Edificio";
                worksheet.Cells[1, 5].Value = "ID Piso";
                worksheet.Cells[1, 6].Value = "Descripción Piso";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var edificioPiso in edificioPisos)
                {
                    worksheet.Cells[row, 1].Value = edificioPiso.IdCompania;
                    worksheet.Cells[row, 2].Value = companias.ContainsKey(edificioPiso.IdCompania) ? companias[edificioPiso.IdCompania] : "";
                    worksheet.Cells[row, 3].Value = edificioPiso.IdEdificio;
                    // Cast to int to lookup in edificios dictionary
                    int edifKey = edificioPiso.IdEdificio;
                    worksheet.Cells[row, 4].Value = edificios.ContainsKey((short)edifKey) ? edificios[(short)edifKey] : "";
                    worksheet.Cells[row, 5].Value = edificioPiso.IdPiso;
                    worksheet.Cells[row, 6].Value = pisos.ContainsKey(edificioPiso.IdPiso) ? pisos[edificioPiso.IdPiso] : "";
                    row++;
                }

                // Set column widths manually (AutoFitColumns not supported on non-Windows)
                worksheet.Column(1).Width = 15;
                worksheet.Column(2).Width = 30;
                worksheet.Column(3).Width = 15;
                worksheet.Column(4).Width = 30;
                worksheet.Column(5).Width = 15;
                worksheet.Column(6).Width = 30;

                // Return the Excel file
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"EdificioPiso_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
        }

        private bool EdificioPisoExists(short idCompania, int idEdificio, short idPiso)
        {
            return _context.EdificioPiso.Any(e => e.IdCompania == idCompania && e.IdEdificio == idEdificio && e.IdPiso == idPiso);
        }
    }
}
