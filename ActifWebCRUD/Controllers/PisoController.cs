using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class PisoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PisoController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Piso
        public async Task<IActionResult> Index()
        {
            var pisos = await _context.Piso.ToListAsync();
            // Note: Compania FK not configured due to type mismatch, load manually if needed
            return View(pisos);
        }

        // GET: Piso/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var piso = await _context.Piso
                .FirstOrDefaultAsync(m => m.IdPiso == id);
            if (piso == null)
            {
                return NotFound();
            }

            return View(piso);
        }

        // GET: Piso/Create
        public IActionResult Create()
        {
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre");
            return View();
        }

        // POST: Piso/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPiso,Descripcion,IdCompania,IdEdificio,IdPisoAnterior")] Piso piso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(piso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre", piso.IdCompania);
            return View(piso);
        }

        // GET: Piso/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var piso = await _context.Piso.FindAsync(id);
            if (piso == null)
            {
                return NotFound();
            }
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre", piso.IdCompania);
            return View(piso);
        }

        // POST: Piso/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("IdPiso,Descripcion,IdCompania,IdEdificio,IdPisoAnterior")] Piso piso)
        {
            if (id != piso.IdPiso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the original rv value
                    var original = await _context.Piso.AsNoTracking().FirstOrDefaultAsync(p => p.IdPiso == id);
                    if (original != null)
                    {
                        piso.Rv = original.Rv;
                    }

                    _context.Update(piso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PisoExists(piso.IdPiso))
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
            ViewData["IdCompania"] = new SelectList(_context.Compania, "IdCompania", "Nombre", piso.IdCompania);
            return View(piso);
        }

        // GET: Piso/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var piso = await _context.Piso
                .FirstOrDefaultAsync(m => m.IdPiso == id);
            if (piso == null)
            {
                return NotFound();
            }

            return View(piso);
        }

        // POST: Piso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var piso = await _context.Piso.FindAsync(id);
            if (piso != null)
            {
                _context.Piso.Remove(piso);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var pisos = await _context.Piso.ToListAsync();

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Pisos");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID Piso";
                worksheet.Cells[1, 2].Value = "Descripcion";
                worksheet.Cells[1, 3].Value = "ID Compania";
                worksheet.Cells[1, 4].Value = "Nombre Compania";
                worksheet.Cells[1, 5].Value = "ID Edificio";
                worksheet.Cells[1, 6].Value = "ID Piso Anterior";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var piso in pisos)
                {
                    worksheet.Cells[row, 1].Value = piso.IdPiso;
                    worksheet.Cells[row, 2].Value = piso.Descripcion;
                    worksheet.Cells[row, 3].Value = piso.IdCompania;
                    // Compania name - manually loaded if needed
                    var compania = piso.IdCompania.HasValue ? _context.Compania.Find((short)piso.IdCompania.Value) : null;
                    worksheet.Cells[row, 4].Value = compania?.Nombre;
                    worksheet.Cells[row, 5].Value = piso.IdEdificio;
                    worksheet.Cells[row, 6].Value = piso.IdPisoAnterior;
                    row++;
                }

                // Set column widths manually (AutoFitColumns not supported on non-Windows)
                for (int col = 1; col <= 6; col++)
                {
                    worksheet.Column(col).Width = 20;
                }

                // Return the Excel file
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Pisos_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
        }

        private bool PisoExists(short id)
        {
            return _context.Piso.Any(e => e.IdPiso == id);
        }
    }
}
