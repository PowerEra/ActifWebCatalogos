using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class TipoDepreciacionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public TipoDepreciacionController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: TipoDepreciacion
        public async Task<IActionResult> Index()
        {
            var tiposDepreciacion = await _context.TipoDepreciacion.ToListAsync();
            return View(tiposDepreciacion);
        }

        // GET: TipoDepreciacion/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDepreciacion = await _context.TipoDepreciacion
                .FirstOrDefaultAsync(m => m.IdTipoDep == id);
            if (tipoDepreciacion == null)
            {
                return NotFound();
            }

            return View(tipoDepreciacion);
        }

        // GET: TipoDepreciacion/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoDepreciacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoDep,Descripcion,MesIni,AplicaFiscal")] TipoDepreciacion tipoDepreciacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoDepreciacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoDepreciacion);
        }

        // GET: TipoDepreciacion/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDepreciacion = await _context.TipoDepreciacion.FindAsync(id);
            if (tipoDepreciacion == null)
            {
                return NotFound();
            }
            return View(tipoDepreciacion);
        }

        // POST: TipoDepreciacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("IdTipoDep,Descripcion,MesIni,AplicaFiscal")] TipoDepreciacion tipoDepreciacion)
        {
            if (id != tipoDepreciacion.IdTipoDep)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoDepreciacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoDepreciacionExists(tipoDepreciacion.IdTipoDep))
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
            return View(tipoDepreciacion);
        }

        // GET: TipoDepreciacion/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDepreciacion = await _context.TipoDepreciacion
                .FirstOrDefaultAsync(m => m.IdTipoDep == id);
            if (tipoDepreciacion == null)
            {
                return NotFound();
            }

            return View(tipoDepreciacion);
        }

        // POST: TipoDepreciacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var tipoDepreciacion = await _context.TipoDepreciacion.FindAsync(id);
            if (tipoDepreciacion != null)
            {
                _context.TipoDepreciacion.Remove(tipoDepreciacion);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel
        public async Task<IActionResult> ExportToExcel()
        {
            var tiposDepreciacion = await _context.TipoDepreciacion.ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("TiposDepreciacion");

                // Headers (row 1)
                worksheet.Cells[1, 1].Value = "ID Tipo Depreciacion";
                worksheet.Cells[1, 2].Value = "Descripcion";
                worksheet.Cells[1, 3].Value = "Mes Inicial";
                worksheet.Cells[1, 4].Value = "Aplica Fiscal";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 4])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Data (starting row 2)
                int row = 2;
                foreach (var tipo in tiposDepreciacion)
                {
                    worksheet.Cells[row, 1].Value = tipo.IdTipoDep;
                    worksheet.Cells[row, 2].Value = tipo.Descripcion;
                    worksheet.Cells[row, 3].Value = tipo.MesIni;
                    worksheet.Cells[row, 4].Value = tipo.AplicaFiscal;
                    row++;
                }

                // Manual column widths (AutoFit doesn't work on macOS)
                worksheet.Column(1).Width = 25;
                worksheet.Column(2).Width = 30;
                worksheet.Column(3).Width = 15;
                worksheet.Column(4).Width = 20;

                return File(package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"TiposDepreciacion_{DateTime.Now:yyyyMMdd}.xlsx");
            }
        }

        private bool TipoDepreciacionExists(short id)
        {
            return _context.TipoDepreciacion.Any(e => e.IdTipoDep == id);
        }
    }
}
