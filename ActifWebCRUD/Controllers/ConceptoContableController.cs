using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class ConceptoContableController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public ConceptoContableController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: ConceptoContable
        public async Task<IActionResult> Index()
        {
            // Use the VIEW for SELECT operations
            var conceptos = await _context.VConceptoContable.ToListAsync();
            return View(conceptos);
        }

        // GET: ConceptoContable/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conceptoContable = await _context.ConceptoContable
                .Include(c => c.Compania)
                .Include(c => c.TipoActivo)
                .Include(c => c.SubtipoActivo)
                .FirstOrDefaultAsync(m => m.IdConcepto == id);
            if (conceptoContable == null)
            {
                return NotFound();
            }

            return View(conceptoContable);
        }

        // GET: ConceptoContable/Create
        public IActionResult Create()
        {
            ViewBag.IdCompania = new SelectList(_context.Compania, "IdCompania", "Nombre");
            ViewBag.IdTipoActivo = new SelectList(_context.TipoActivo, "IdTipoActivo", "Descripcion");
            ViewBag.IdSubtipoActivo = new SelectList(_context.SubtipoActivo, "IdSubtipoActivo", "Descripcion");
            return View();
        }

        // POST: ConceptoContable/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdConcepto,IdCompania,IdTipoActivo,IdSubtipoActivo,IdVariable,Descripcion,Cta101,Cta102,Cta103,Cta104,Cta105,Cta106,Caab01,Desg01,Pror01,Cta201,Cta202,Cta203,Cta204,Cta205,Cta206,Caab02,Desg02,Pror02,Cta301,Cta302,Cta303,Cta304,Cta305,Cta306,Caab03,Desg03,Pror03,IdEdificio,Descripcion2,Descripcion200")] ConceptoContable conceptoContable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conceptoContable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.IdCompania = new SelectList(_context.Compania, "IdCompania", "Nombre", conceptoContable.IdCompania);
            ViewBag.IdTipoActivo = new SelectList(_context.TipoActivo, "IdTipoActivo", "Descripcion", conceptoContable.IdTipoActivo);
            ViewBag.IdSubtipoActivo = new SelectList(_context.SubtipoActivo, "IdSubtipoActivo", "Descripcion", conceptoContable.IdSubtipoActivo);
            return View(conceptoContable);
        }

        // GET: ConceptoContable/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conceptoContable = await _context.ConceptoContable.FindAsync(id);
            if (conceptoContable == null)
            {
                return NotFound();
            }
            ViewBag.IdCompania = new SelectList(_context.Compania, "IdCompania", "Nombre", conceptoContable.IdCompania);
            ViewBag.IdTipoActivo = new SelectList(_context.TipoActivo, "IdTipoActivo", "Descripcion", conceptoContable.IdTipoActivo);
            ViewBag.IdSubtipoActivo = new SelectList(_context.SubtipoActivo, "IdSubtipoActivo", "Descripcion", conceptoContable.IdSubtipoActivo);
            return View(conceptoContable);
        }

        // POST: ConceptoContable/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("IdConcepto,IdCompania,IdTipoActivo,IdSubtipoActivo,IdVariable,Descripcion,Cta101,Cta102,Cta103,Cta104,Cta105,Cta106,Caab01,Desg01,Pror01,Cta201,Cta202,Cta203,Cta204,Cta205,Cta206,Caab02,Desg02,Pror02,Cta301,Cta302,Cta303,Cta304,Cta305,Cta306,Caab03,Desg03,Pror03,IdEdificio,Descripcion2,Descripcion200")] ConceptoContable conceptoContable)
        {
            if (id != conceptoContable.IdConcepto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conceptoContable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConceptoContableExists(conceptoContable.IdConcepto))
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
            ViewBag.IdCompania = new SelectList(_context.Compania, "IdCompania", "Nombre", conceptoContable.IdCompania);
            ViewBag.IdTipoActivo = new SelectList(_context.TipoActivo, "IdTipoActivo", "Descripcion", conceptoContable.IdTipoActivo);
            ViewBag.IdSubtipoActivo = new SelectList(_context.SubtipoActivo, "IdSubtipoActivo", "Descripcion", conceptoContable.IdSubtipoActivo);
            return View(conceptoContable);
        }

        // GET: ConceptoContable/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conceptoContable = await _context.ConceptoContable
                .Include(c => c.Compania)
                .Include(c => c.TipoActivo)
                .Include(c => c.SubtipoActivo)
                .FirstOrDefaultAsync(m => m.IdConcepto == id);
            if (conceptoContable == null)
            {
                return NotFound();
            }

            return View(conceptoContable);
        }

        // POST: ConceptoContable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var conceptoContable = await _context.ConceptoContable.FindAsync(id);
            if (conceptoContable != null)
            {
                _context.ConceptoContable.Remove(conceptoContable);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var conceptos = await _context.VConceptoContable.ToListAsync();

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Conceptos Contables");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID Concepto";
                worksheet.Cells[1, 2].Value = "Compañía";
                worksheet.Cells[1, 3].Value = "Tipo Activo";
                worksheet.Cells[1, 4].Value = "Subtipo Activo";
                worksheet.Cells[1, 5].Value = "Variable";
                worksheet.Cells[1, 6].Value = "Descripción";
                worksheet.Cells[1, 7].Value = "CTA101";
                worksheet.Cells[1, 8].Value = "CTA102";
                worksheet.Cells[1, 9].Value = "CTA103";
                worksheet.Cells[1, 10].Value = "CTA104";
                worksheet.Cells[1, 11].Value = "CTA105";
                worksheet.Cells[1, 12].Value = "CTA106";
                worksheet.Cells[1, 13].Value = "CAAB01";
                worksheet.Cells[1, 14].Value = "DESG01";
                worksheet.Cells[1, 15].Value = "PROR01";
                worksheet.Cells[1, 16].Value = "Edificio";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 16])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var concepto in conceptos)
                {
                    worksheet.Cells[row, 1].Value = concepto.IdConcepto;
                    worksheet.Cells[row, 2].Value = concepto.Compania;
                    worksheet.Cells[row, 3].Value = concepto.TipoActivo;
                    worksheet.Cells[row, 4].Value = concepto.SubtipoActivo;
                    worksheet.Cells[row, 5].Value = concepto.Variable;
                    worksheet.Cells[row, 6].Value = concepto.Descripcion;
                    worksheet.Cells[row, 7].Value = concepto.Cta101;
                    worksheet.Cells[row, 8].Value = concepto.Cta102;
                    worksheet.Cells[row, 9].Value = concepto.Cta103;
                    worksheet.Cells[row, 10].Value = concepto.Cta104;
                    worksheet.Cells[row, 11].Value = concepto.Cta105;
                    worksheet.Cells[row, 12].Value = concepto.Cta106;
                    worksheet.Cells[row, 13].Value = concepto.Caab01;
                    worksheet.Cells[row, 14].Value = concepto.Desg01;
                    worksheet.Cells[row, 15].Value = concepto.Pror01;
                    worksheet.Cells[row, 16].Value = concepto.Edificio;
                    row++;
                }

                // Set column widths manually to avoid System.Drawing issues on macOS
                for (int col = 1; col <= 16; col++)
                {
                    worksheet.Column(col).Width = 20;
                }

                // Return the Excel file
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"ConceptosContables_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
        }

        private bool ConceptoContableExists(short id)
        {
            return _context.ConceptoContable.Any(e => e.IdConcepto == id);
        }
    }
}
