using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class CompaniaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public CompaniaController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Compania
        public async Task<IActionResult> Index()
        {
            var companias = await _context.Compania.ToListAsync();
            return View(companias);
        }

        // GET: Compania/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compania = await _context.Compania
                .FirstOrDefaultAsync(m => m.IdCompania == id);
            if (compania == null)
            {
                return NotFound();
            }

            return View(compania);
        }

        // GET: Compania/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Compania/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCompania,Nombre,Rfc,FechaInicioEjerc,IdPolizaSig,ValorCero,CalleNumero,Colonia,DelegMpio,CodigoPostal,IdEstado,IdSigResponsiva,Telefono,Cuenta,IdContable,Division,DefaultIdMoneda,DefaultIdPais,DefaultTipoCambio,IdTipoDepPrincipal,RequerirDocumento,RequerirAprobacion,PrefijoRfid")] Compania compania)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compania);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(compania);
        }

        // GET: Compania/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compania = await _context.Compania.FindAsync(id);
            if (compania == null)
            {
                return NotFound();
            }
            return View(compania);
        }

        // POST: Compania/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("IdCompania,Nombre,Rfc,FechaInicioEjerc,IdPolizaSig,ValorCero,CalleNumero,Colonia,DelegMpio,CodigoPostal,IdEstado,IdSigResponsiva,Telefono,Cuenta,IdContable,Division,DefaultIdMoneda,DefaultIdPais,DefaultTipoCambio,IdTipoDepPrincipal,RequerirDocumento,RequerirAprobacion,PrefijoRfid")] Compania compania)
        {
            if (id != compania.IdCompania)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the original rv value
                    var original = await _context.Compania.AsNoTracking().FirstOrDefaultAsync(c => c.IdCompania == id);
                    if (original != null)
                    {
                        compania.Rv = original.Rv;
                    }

                    _context.Update(compania);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompaniaExists(compania.IdCompania))
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
            return View(compania);
        }

        // GET: Compania/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compania = await _context.Compania
                .FirstOrDefaultAsync(m => m.IdCompania == id);
            if (compania == null)
            {
                return NotFound();
            }

            return View(compania);
        }

        // POST: Compania/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var compania = await _context.Compania.FindAsync(id);
            if (compania != null)
            {
                _context.Compania.Remove(compania);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel
        public async Task<IActionResult> ExportToExcel()
        {
            var companias = await _context.Compania.ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Companias");

                // Headers (row 1)
                worksheet.Cells[1, 1].Value = "ID Compania";
                worksheet.Cells[1, 2].Value = "Nombre";
                worksheet.Cells[1, 3].Value = "RFC";
                worksheet.Cells[1, 4].Value = "Fecha Inicio Ejercicio";
                worksheet.Cells[1, 5].Value = "Calle y Numero";
                worksheet.Cells[1, 6].Value = "Colonia";
                worksheet.Cells[1, 7].Value = "Delegacion/Municipio";
                worksheet.Cells[1, 8].Value = "Codigo Postal";
                worksheet.Cells[1, 9].Value = "Telefono";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 9])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Data (starting row 2)
                int row = 2;
                foreach (var compania in companias)
                {
                    worksheet.Cells[row, 1].Value = compania.IdCompania;
                    worksheet.Cells[row, 2].Value = compania.Nombre;
                    worksheet.Cells[row, 3].Value = compania.Rfc;
                    worksheet.Cells[row, 4].Value = compania.FechaInicioEjerc?.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 5].Value = compania.CalleNumero;
                    worksheet.Cells[row, 6].Value = compania.Colonia;
                    worksheet.Cells[row, 7].Value = compania.DelegMpio;
                    worksheet.Cells[row, 8].Value = compania.CodigoPostal;
                    worksheet.Cells[row, 9].Value = compania.Telefono;
                    row++;
                }

                // Manual column widths (AutoFit doesn't work on macOS)
                worksheet.Column(1).Width = 15;
                worksheet.Column(2).Width = 30;
                worksheet.Column(3).Width = 20;
                worksheet.Column(4).Width = 25;
                worksheet.Column(5).Width = 30;
                worksheet.Column(6).Width = 25;
                worksheet.Column(7).Width = 30;
                worksheet.Column(8).Width = 15;
                worksheet.Column(9).Width = 20;

                return File(package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Companias_{DateTime.Now:yyyyMMdd}.xlsx");
            }
        }

        private bool CompaniaExists(short id)
        {
            return _context.Compania.Any(e => e.IdCompania == id);
        }
    }
}
