using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class ActifTipoDepEdificioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public ActifTipoDepEdificioController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: ActifTipoDepEdificio
        public async Task<IActionResult> Index()
        {
            var actifTipoDepEdificios = await _context.ActifTipoDepEdificio.ToListAsync();

            // Manually load navigation properties
            foreach (var item in actifTipoDepEdificios)
            {
                if (item.IdTipoDep.HasValue)
                {
                    item.TipoDepreciacion = await _context.TipoDepreciacion.FindAsync(item.IdTipoDep.Value);
                }
                if (item.IdEdificio.HasValue)
                {
                    item.Edificio = await _context.Edificio.FindAsync(item.IdEdificio.Value);
                }
            }

            return View(actifTipoDepEdificios);
        }

        // GET: ActifTipoDepEdificio/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actifTipoDepEdificio = await _context.ActifTipoDepEdificio
                .FirstOrDefaultAsync(m => m.Idx == id);

            if (actifTipoDepEdificio == null)
            {
                return NotFound();
            }

            // Load navigation properties
            if (actifTipoDepEdificio.IdTipoDep.HasValue)
            {
                actifTipoDepEdificio.TipoDepreciacion = await _context.TipoDepreciacion.FindAsync(actifTipoDepEdificio.IdTipoDep.Value);
            }
            if (actifTipoDepEdificio.IdEdificio.HasValue)
            {
                actifTipoDepEdificio.Edificio = await _context.Edificio.FindAsync(actifTipoDepEdificio.IdEdificio.Value);
            }

            return View(actifTipoDepEdificio);
        }

        // GET: ActifTipoDepEdificio/Create
        public IActionResult Create()
        {
            ViewData["IdTipoDep"] = new SelectList(_context.TipoDepreciacion.OrderBy(t => t.Descripcion), "IdTipoDep", "Descripcion");
            ViewData["IdEdificio"] = new SelectList(_context.Edificio.OrderBy(e => e.Descripcion), "IdEdificio", "Descripcion");
            return View();
        }

        // POST: ActifTipoDepEdificio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoDep,IdEdificio,FechaCaptura")] ActifTipoDepEdificio actifTipoDepEdificio)
        {
            if (ModelState.IsValid)
            {
                // Set FechaCaptura to current date/time if not provided
                if (!actifTipoDepEdificio.FechaCaptura.HasValue)
                {
                    actifTipoDepEdificio.FechaCaptura = DateTime.Now;
                }

                _context.Add(actifTipoDepEdificio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTipoDep"] = new SelectList(_context.TipoDepreciacion.OrderBy(t => t.Descripcion), "IdTipoDep", "Descripcion", actifTipoDepEdificio.IdTipoDep);
            ViewData["IdEdificio"] = new SelectList(_context.Edificio.OrderBy(e => e.Descripcion), "IdEdificio", "Descripcion", actifTipoDepEdificio.IdEdificio);
            return View(actifTipoDepEdificio);
        }

        // GET: ActifTipoDepEdificio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actifTipoDepEdificio = await _context.ActifTipoDepEdificio.FindAsync(id);
            if (actifTipoDepEdificio == null)
            {
                return NotFound();
            }

            ViewData["IdTipoDep"] = new SelectList(_context.TipoDepreciacion.OrderBy(t => t.Descripcion), "IdTipoDep", "Descripcion", actifTipoDepEdificio.IdTipoDep);
            ViewData["IdEdificio"] = new SelectList(_context.Edificio.OrderBy(e => e.Descripcion), "IdEdificio", "Descripcion", actifTipoDepEdificio.IdEdificio);
            return View(actifTipoDepEdificio);
        }

        // POST: ActifTipoDepEdificio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idx,IdTipoDep,IdEdificio,FechaCaptura")] ActifTipoDepEdificio actifTipoDepEdificio)
        {
            if (id != actifTipoDepEdificio.Idx)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actifTipoDepEdificio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActifTipoDepEdificioExists(actifTipoDepEdificio.Idx))
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
            ViewData["IdTipoDep"] = new SelectList(_context.TipoDepreciacion.OrderBy(t => t.Descripcion), "IdTipoDep", "Descripcion", actifTipoDepEdificio.IdTipoDep);
            ViewData["IdEdificio"] = new SelectList(_context.Edificio.OrderBy(e => e.Descripcion), "IdEdificio", "Descripcion", actifTipoDepEdificio.IdEdificio);
            return View(actifTipoDepEdificio);
        }

        // GET: ActifTipoDepEdificio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actifTipoDepEdificio = await _context.ActifTipoDepEdificio
                .FirstOrDefaultAsync(m => m.Idx == id);

            if (actifTipoDepEdificio == null)
            {
                return NotFound();
            }

            // Load navigation properties
            if (actifTipoDepEdificio.IdTipoDep.HasValue)
            {
                actifTipoDepEdificio.TipoDepreciacion = await _context.TipoDepreciacion.FindAsync(actifTipoDepEdificio.IdTipoDep.Value);
            }
            if (actifTipoDepEdificio.IdEdificio.HasValue)
            {
                actifTipoDepEdificio.Edificio = await _context.Edificio.FindAsync(actifTipoDepEdificio.IdEdificio.Value);
            }

            return View(actifTipoDepEdificio);
        }

        // POST: ActifTipoDepEdificio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actifTipoDepEdificio = await _context.ActifTipoDepEdificio.FindAsync(id);
            if (actifTipoDepEdificio != null)
            {
                _context.ActifTipoDepEdificio.Remove(actifTipoDepEdificio);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var actifTipoDepEdificios = await _context.ActifTipoDepEdificio.ToListAsync();

            // Manually load navigation properties for each item
            foreach (var item in actifTipoDepEdificios)
            {
                if (item.IdTipoDep.HasValue)
                {
                    item.TipoDepreciacion = await _context.TipoDepreciacion.FindAsync(item.IdTipoDep.Value);
                }
                if (item.IdEdificio.HasValue)
                {
                    item.Edificio = await _context.Edificio.FindAsync(item.IdEdificio.Value);
                }
            }

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Tipo Depreciacion Edificio");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "ID Tipo Depreciación";
                worksheet.Cells[1, 3].Value = "Descripción Tipo Depreciación";
                worksheet.Cells[1, 4].Value = "ID Edificio";
                worksheet.Cells[1, 5].Value = "Descripción Edificio";
                worksheet.Cells[1, 6].Value = "Fecha Captura";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var item in actifTipoDepEdificios)
                {
                    worksheet.Cells[row, 1].Value = item.Idx;
                    worksheet.Cells[row, 2].Value = item.IdTipoDep;
                    worksheet.Cells[row, 3].Value = item.TipoDepreciacion?.Descripcion ?? "";
                    worksheet.Cells[row, 4].Value = item.IdEdificio;
                    worksheet.Cells[row, 5].Value = item.Edificio?.Descripcion ?? "";
                    worksheet.Cells[row, 6].Value = item.FechaCaptura.HasValue
                        ? item.FechaCaptura.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
                    row++;
                }

                // Set column widths manually (AutoFitColumns not supported on non-Windows)
                worksheet.Column(1).Width = 10;
                worksheet.Column(2).Width = 20;
                worksheet.Column(3).Width = 35;
                worksheet.Column(4).Width = 15;
                worksheet.Column(5).Width = 35;
                worksheet.Column(6).Width = 20;

                // Return the Excel file
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"TipoDepEdificio_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
        }

        private bool ActifTipoDepEdificioExists(int id)
        {
            return _context.ActifTipoDepEdificio.Any(e => e.Idx == id);
        }
    }
}
