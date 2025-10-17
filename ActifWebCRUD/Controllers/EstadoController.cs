using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class EstadoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public EstadoController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Estado
        public async Task<IActionResult> Index()
        {
            // Use vEstado view for Index display
            var estados = await _context.VEstado.ToListAsync();
            return View(estados);
        }

        // GET: Estado/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estado = await _context.Estado.FirstOrDefaultAsync(m => m.IdEstado == id);
            if (estado == null)
            {
                return NotFound();
            }

            // Manually load Pais due to type mismatch (estado.ID_PAIS is smallint, pais.ID_PAIS is int)
            if (estado.IdPais.HasValue)
            {
                estado.Pais = await _context.Pais.FindAsync((int)estado.IdPais.Value);
            }

            return View(estado);
        }

        // GET: Estado/Create
        public IActionResult Create()
        {
            PopulatePaisDropdown();
            return View();
        }

        // POST: Estado/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,IdPais")] Estado estado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulatePaisDropdown(estado.IdPais);
            return View(estado);
        }

        // GET: Estado/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estado = await _context.Estado.FindAsync(id);
            if (estado == null)
            {
                return NotFound();
            }
            PopulatePaisDropdown(estado.IdPais);
            return View(estado);
        }

        // POST: Estado/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("IdEstado,Nombre,IdPais")] Estado estado)
        {
            if (id != estado.IdEstado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoExists(estado.IdEstado))
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
            PopulatePaisDropdown(estado.IdPais);
            return View(estado);
        }

        // GET: Estado/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estado = await _context.Estado.FirstOrDefaultAsync(m => m.IdEstado == id);
            if (estado == null)
            {
                return NotFound();
            }

            // Manually load Pais due to type mismatch (estado.ID_PAIS is smallint, pais.ID_PAIS is int)
            if (estado.IdPais.HasValue)
            {
                estado.Pais = await _context.Pais.FindAsync((int)estado.IdPais.Value);
            }

            return View(estado);
        }

        // POST: Estado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var estado = await _context.Estado.FindAsync(id);
            if (estado != null)
            {
                _context.Estado.Remove(estado);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel
        public async Task<IActionResult> ExportToExcel()
        {
            var estados = await _context.VEstado.ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Estados");

                // Headers (row 1)
                worksheet.Cells[1, 1].Value = "ID Estado";
                worksheet.Cells[1, 2].Value = "Pais";
                worksheet.Cells[1, 3].Value = "Nombre";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 3])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Data (starting row 2)
                int row = 2;
                foreach (var estado in estados)
                {
                    worksheet.Cells[row, 1].Value = estado.IdEstado;
                    worksheet.Cells[row, 2].Value = estado.Pais;
                    worksheet.Cells[row, 3].Value = estado.Nombre;
                    row++;
                }

                // Manual column widths (AutoFit doesn't work on macOS)
                worksheet.Column(1).Width = 15;
                worksheet.Column(2).Width = 30;
                worksheet.Column(3).Width = 30;

                return File(package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Estados_{DateTime.Now:yyyyMMdd}.xlsx");
            }
        }

        private void PopulatePaisDropdown(object? selectedValue = null)
        {
            var paises = _context.Pais
                .OrderBy(p => p.Nombre)
                .Select(p => new SelectListItem
                {
                    Value = p.IdPais.ToString(),
                    Text = p.Nombre
                })
                .ToList();

            paises.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione un Pais --" });

            ViewBag.IdPais = new SelectList(paises, "Value", "Text", selectedValue);
        }

        private bool EstadoExists(short id)
        {
            return _context.Estado.Any(e => e.IdEstado == id);
        }
    }
}
