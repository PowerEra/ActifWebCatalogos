using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class EdificioCentroCostoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public EdificioCentroCostoController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: EdificioCentroCosto
        public async Task<IActionResult> Index()
        {
            var edificioCentroCostos = await _context.EdificioCentroCosto.ToListAsync();

            // Pre-load all related data to avoid individual lookups
            var companias = await _context.Compania.ToDictionaryAsync(c => c.IdCompania);
            var edificios = await _context.Edificio.ToDictionaryAsync(e => e.IdEdificio);
            var centroCostos = await _context.CentroCosto.ToDictionaryAsync(c => c.IdCentroCosto);

            // Manually load navigation properties
            foreach (var ecc in edificioCentroCostos)
            {
                if (companias.ContainsKey(ecc.IdCompania))
                    ecc.Compania = companias[ecc.IdCompania];
                if (edificios.ContainsKey(ecc.IdEdificio))
                    ecc.Edificio = edificios[ecc.IdEdificio];
                if (centroCostos.ContainsKey(ecc.IdCentroCosto))
                    ecc.CentroCosto = centroCostos[ecc.IdCentroCosto];
            }

            return View(edificioCentroCostos);
        }

        // GET: EdificioCentroCosto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var edificioCentroCosto = await _context.EdificioCentroCosto
                .FirstOrDefaultAsync(m => m.IdEdificioCentroCosto == id);

            if (edificioCentroCosto == null)
            {
                return NotFound();
            }

            // Load navigation properties
            edificioCentroCosto.Compania = await _context.Compania.FirstOrDefaultAsync(c => c.IdCompania == edificioCentroCosto.IdCompania);
            edificioCentroCosto.Edificio = await _context.Edificio.FirstOrDefaultAsync(e => e.IdEdificio == edificioCentroCosto.IdEdificio);
            edificioCentroCosto.CentroCosto = await _context.CentroCosto.FirstOrDefaultAsync(c => c.IdCentroCosto == edificioCentroCosto.IdCentroCosto);

            return View(edificioCentroCosto);
        }

        // GET: EdificioCentroCosto/Create
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

            // Get centro costos and manually create list items
            var centroCostos = _context.CentroCosto.OrderBy(c => c.Descripcion).ToList();
            var centroCostoItems = centroCostos.Select(c => new SelectListItem
            {
                Value = c.IdCentroCosto.ToString(),
                Text = $"{c.Codigo} - {c.Descripcion}"
            }).ToList();
            ViewData["IdCentroCosto"] = centroCostoItems;

            return View();
        }

        // POST: EdificioCentroCosto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCompania,IdEdificio,IdCentroCosto")] EdificioCentroCosto edificioCentroCosto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(edificioCentroCosto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre", edificioCentroCosto.IdCompania);

            var edificios = _context.Edificio.OrderBy(e => e.Descripcion).ToList();
            var edificioItems = edificios.Select(e => new SelectListItem
            {
                Value = e.IdEdificio.ToString(),
                Text = e.Descripcion ?? e.IdEdificio.ToString(),
                Selected = e.IdEdificio == edificioCentroCosto.IdEdificio
            }).ToList();
            ViewData["IdEdificio"] = edificioItems;

            var centroCostos = _context.CentroCosto.OrderBy(c => c.Descripcion).ToList();
            var centroCostoItems = centroCostos.Select(c => new SelectListItem
            {
                Value = c.IdCentroCosto.ToString(),
                Text = $"{c.Codigo} - {c.Descripcion}",
                Selected = c.IdCentroCosto == edificioCentroCosto.IdCentroCosto
            }).ToList();
            ViewData["IdCentroCosto"] = centroCostoItems;

            return View(edificioCentroCosto);
        }

        // GET: EdificioCentroCosto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var edificioCentroCosto = await _context.EdificioCentroCosto.FindAsync(id);
            if (edificioCentroCosto == null)
            {
                return NotFound();
            }

            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre", edificioCentroCosto.IdCompania);

            var edificios = _context.Edificio.OrderBy(e => e.Descripcion).ToList();
            var edificioItems = edificios.Select(e => new SelectListItem
            {
                Value = e.IdEdificio.ToString(),
                Text = e.Descripcion ?? e.IdEdificio.ToString(),
                Selected = e.IdEdificio == edificioCentroCosto.IdEdificio
            }).ToList();
            ViewData["IdEdificio"] = edificioItems;

            var centroCostos = _context.CentroCosto.OrderBy(c => c.Descripcion).ToList();
            var centroCostoItems = centroCostos.Select(c => new SelectListItem
            {
                Value = c.IdCentroCosto.ToString(),
                Text = $"{c.Codigo} - {c.Descripcion}",
                Selected = c.IdCentroCosto == edificioCentroCosto.IdCentroCosto
            }).ToList();
            ViewData["IdCentroCosto"] = centroCostoItems;

            return View(edificioCentroCosto);
        }

        // POST: EdificioCentroCosto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEdificioCentroCosto,IdCompania,IdEdificio,IdCentroCosto")] EdificioCentroCosto edificioCentroCosto)
        {
            if (id != edificioCentroCosto.IdEdificioCentroCosto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(edificioCentroCosto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EdificioCentroCostoExists(edificioCentroCosto.IdEdificioCentroCosto))
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

            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre", edificioCentroCosto.IdCompania);

            var edificios = _context.Edificio.OrderBy(e => e.Descripcion).ToList();
            var edificioItems = edificios.Select(e => new SelectListItem
            {
                Value = e.IdEdificio.ToString(),
                Text = e.Descripcion ?? e.IdEdificio.ToString(),
                Selected = e.IdEdificio == edificioCentroCosto.IdEdificio
            }).ToList();
            ViewData["IdEdificio"] = edificioItems;

            var centroCostos = _context.CentroCosto.OrderBy(c => c.Descripcion).ToList();
            var centroCostoItems = centroCostos.Select(c => new SelectListItem
            {
                Value = c.IdCentroCosto.ToString(),
                Text = $"{c.Codigo} - {c.Descripcion}",
                Selected = c.IdCentroCosto == edificioCentroCosto.IdCentroCosto
            }).ToList();
            ViewData["IdCentroCosto"] = centroCostoItems;

            return View(edificioCentroCosto);
        }

        // GET: EdificioCentroCosto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var edificioCentroCosto = await _context.EdificioCentroCosto
                .FirstOrDefaultAsync(m => m.IdEdificioCentroCosto == id);

            if (edificioCentroCosto == null)
            {
                return NotFound();
            }

            // Load navigation properties
            edificioCentroCosto.Compania = await _context.Compania.FirstOrDefaultAsync(c => c.IdCompania == edificioCentroCosto.IdCompania);
            edificioCentroCosto.Edificio = await _context.Edificio.FirstOrDefaultAsync(e => e.IdEdificio == edificioCentroCosto.IdEdificio);
            edificioCentroCosto.CentroCosto = await _context.CentroCosto.FirstOrDefaultAsync(c => c.IdCentroCosto == edificioCentroCosto.IdCentroCosto);

            return View(edificioCentroCosto);
        }

        // POST: EdificioCentroCosto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var edificioCentroCosto = await _context.EdificioCentroCosto.FindAsync(id);
            if (edificioCentroCosto != null)
            {
                _context.EdificioCentroCosto.Remove(edificioCentroCosto);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var edificioCentroCostos = await _context.EdificioCentroCosto.ToListAsync();

            // Pre-load all related data
            var companias = await _context.Compania.ToDictionaryAsync(c => c.IdCompania, c => c.Nombre);
            var edificios = await _context.Edificio.ToDictionaryAsync(e => e.IdEdificio, e => e.Descripcion);
            var centroCostos = await _context.CentroCosto.ToDictionaryAsync(c => c.IdCentroCosto, c => $"{c.Codigo} - {c.Descripcion}");

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Edificio Centro Costo");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID Edificio Centro Costo";
                worksheet.Cells[1, 2].Value = "ID Compañía";
                worksheet.Cells[1, 3].Value = "Nombre Compañía";
                worksheet.Cells[1, 4].Value = "ID Edificio";
                worksheet.Cells[1, 5].Value = "Descripción Edificio";
                worksheet.Cells[1, 6].Value = "ID Centro Costo";
                worksheet.Cells[1, 7].Value = "Centro Costo";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 7])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var ecc in edificioCentroCostos)
                {
                    worksheet.Cells[row, 1].Value = ecc.IdEdificioCentroCosto;
                    worksheet.Cells[row, 2].Value = ecc.IdCompania;
                    worksheet.Cells[row, 3].Value = companias.ContainsKey(ecc.IdCompania) ? companias[ecc.IdCompania] : "";
                    worksheet.Cells[row, 4].Value = ecc.IdEdificio;
                    worksheet.Cells[row, 5].Value = edificios.ContainsKey(ecc.IdEdificio) ? edificios[ecc.IdEdificio] : "";
                    worksheet.Cells[row, 6].Value = ecc.IdCentroCosto;
                    worksheet.Cells[row, 7].Value = centroCostos.ContainsKey(ecc.IdCentroCosto) ? centroCostos[ecc.IdCentroCosto] : "";
                    row++;
                }

                // Set column widths
                worksheet.Column(1).Width = 25;
                worksheet.Column(2).Width = 15;
                worksheet.Column(3).Width = 30;
                worksheet.Column(4).Width = 15;
                worksheet.Column(5).Width = 30;
                worksheet.Column(6).Width = 20;
                worksheet.Column(7).Width = 35;

                // Return the Excel file
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"EdificioCentroCosto_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
        }

        private bool EdificioCentroCostoExists(int id)
        {
            return _context.EdificioCentroCosto.Any(e => e.IdEdificioCentroCosto == id);
        }
    }
}
