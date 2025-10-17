using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class INPC2Controller : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public INPC2Controller(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: INPC2
        public async Task<IActionResult> Index()
        {
            var inpc2List = await _context.INPC2
                .Include(i => i.GrupoSimulacion)
                .Include(i => i.Pais)
                .ToListAsync();
            return View(inpc2List);
        }

        // GET: INPC2/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inpc2 = await _context.INPC2
                .Include(i => i.GrupoSimulacion)
                .Include(i => i.Pais)
                .FirstOrDefaultAsync(m => m.IdInpc == id);
            if (inpc2 == null)
            {
                return NotFound();
            }

            return View(inpc2);
        }

        // GET: INPC2/Create
        public IActionResult Create()
        {
            ViewData["IdGrupoSimulacion"] = new SelectList(_context.GrupoSimulacion, "IdGrupoSimulacion", "Descripcion");
            ViewData["IdPais"] = new SelectList(_context.Pais, "IdPais", "Nombre");
            return View();
        }

        // POST: INPC2/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Anio,Mes,IdGrupoSimulacion,IdPais,Indice")] INPC2 inpc2)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inpc2);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdGrupoSimulacion"] = new SelectList(_context.GrupoSimulacion, "IdGrupoSimulacion", "Descripcion", inpc2.IdGrupoSimulacion);
            ViewData["IdPais"] = new SelectList(_context.Pais, "IdPais", "Nombre", inpc2.IdPais);
            return View(inpc2);
        }

        // GET: INPC2/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inpc2 = await _context.INPC2.FindAsync(id);
            if (inpc2 == null)
            {
                return NotFound();
            }
            ViewData["IdGrupoSimulacion"] = new SelectList(_context.GrupoSimulacion, "IdGrupoSimulacion", "Descripcion", inpc2.IdGrupoSimulacion);
            ViewData["IdPais"] = new SelectList(_context.Pais, "IdPais", "Nombre", inpc2.IdPais);
            return View(inpc2);
        }

        // POST: INPC2/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdInpc,Anio,Mes,IdGrupoSimulacion,IdPais,Indice")] INPC2 inpc2)
        {
            if (id != inpc2.IdInpc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inpc2);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!INPC2Exists(inpc2.IdInpc))
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
            ViewData["IdGrupoSimulacion"] = new SelectList(_context.GrupoSimulacion, "IdGrupoSimulacion", "Descripcion", inpc2.IdGrupoSimulacion);
            ViewData["IdPais"] = new SelectList(_context.Pais, "IdPais", "Nombre", inpc2.IdPais);
            return View(inpc2);
        }

        // GET: INPC2/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inpc2 = await _context.INPC2
                .Include(i => i.GrupoSimulacion)
                .Include(i => i.Pais)
                .FirstOrDefaultAsync(m => m.IdInpc == id);
            if (inpc2 == null)
            {
                return NotFound();
            }

            return View(inpc2);
        }

        // POST: INPC2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inpc2 = await _context.INPC2.FindAsync(id);
            if (inpc2 != null)
            {
                _context.INPC2.Remove(inpc2);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var inpc2List = await _context.INPC2
                .Include(i => i.GrupoSimulacion)
                .Include(i => i.Pais)
                .ToListAsync();

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("INPC2");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID INPC";
                worksheet.Cells[1, 2].Value = "Año";
                worksheet.Cells[1, 3].Value = "Mes";
                worksheet.Cells[1, 4].Value = "Grupo Simulación";
                worksheet.Cells[1, 5].Value = "País";
                worksheet.Cells[1, 6].Value = "Índice";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var inpc in inpc2List)
                {
                    worksheet.Cells[row, 1].Value = inpc.IdInpc;
                    worksheet.Cells[row, 2].Value = inpc.Anio;
                    worksheet.Cells[row, 3].Value = inpc.Mes;
                    worksheet.Cells[row, 4].Value = inpc.GrupoSimulacion?.Descripcion;
                    worksheet.Cells[row, 5].Value = inpc.Pais?.Nombre;
                    worksheet.Cells[row, 6].Value = inpc.Indice;
                    row++;
                }

                // Set column widths manually (AutoFitColumns requires System.Drawing on macOS)
                worksheet.Column(1).Width = 15;
                worksheet.Column(2).Width = 15;
                worksheet.Column(3).Width = 10;
                worksheet.Column(4).Width = 25;
                worksheet.Column(5).Width = 30;
                worksheet.Column(6).Width = 20;

                // Return the Excel file
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"INPC2_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
        }

        private bool INPC2Exists(int id)
        {
            return _context.INPC2.Any(e => e.IdInpc == id);
        }
    }
}
