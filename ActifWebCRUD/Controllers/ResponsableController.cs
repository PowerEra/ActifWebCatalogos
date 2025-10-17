using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class ResponsableController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResponsableController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Responsable
        public async Task<IActionResult> Index()
        {
            // Use vResponsable view for Index display
            var responsables = await _context.VResponsable.ToListAsync();
            return View(responsables);
        }

        // GET: Responsable/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var responsable = await _context.Responsable.FirstOrDefaultAsync(m => m.IdResponsable == id);
            if (responsable == null)
            {
                return NotFound();
            }

            // Load related data for display
            if (responsable.IdCompania.HasValue)
            {
                var compania = await _context.Compania.FindAsync(responsable.IdCompania.Value);
                ViewBag.CompaniaNombre = compania?.Nombre ?? "";
            }

            if (responsable.IdCentroCosto.HasValue)
            {
                var centroCosto = await _context.VCentroCosto.FirstOrDefaultAsync(c => c.IdCentroCosto == responsable.IdCentroCosto.Value);
                ViewBag.CentroCostoNombre = centroCosto?.Descripcion ?? "";
            }

            return View(responsable);
        }

        // GET: Responsable/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Responsable/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCentroCosto,IdCompania,Nombre,PuestoResponsable,NumeroEmpleado")] Responsable responsable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(responsable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDropdowns(responsable.IdCompania, responsable.IdCentroCosto);
            return View(responsable);
        }

        // GET: Responsable/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var responsable = await _context.Responsable.FindAsync(id);
            if (responsable == null)
            {
                return NotFound();
            }
            PopulateDropdowns(responsable.IdCompania, responsable.IdCentroCosto);
            return View(responsable);
        }

        // POST: Responsable/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdResponsable,IdCentroCosto,IdCompania,Nombre,PuestoResponsable,NumeroEmpleado,Rv")] Responsable responsable)
        {
            if (id != responsable.IdResponsable)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(responsable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResponsableExists(responsable.IdResponsable))
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
            PopulateDropdowns(responsable.IdCompania, responsable.IdCentroCosto);
            return View(responsable);
        }

        // GET: Responsable/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var responsable = await _context.Responsable.FirstOrDefaultAsync(m => m.IdResponsable == id);
            if (responsable == null)
            {
                return NotFound();
            }

            // Load related data for display
            if (responsable.IdCompania.HasValue)
            {
                var compania = await _context.Compania.FindAsync(responsable.IdCompania.Value);
                ViewBag.CompaniaNombre = compania?.Nombre ?? "";
            }

            if (responsable.IdCentroCosto.HasValue)
            {
                var centroCosto = await _context.VCentroCosto.FirstOrDefaultAsync(c => c.IdCentroCosto == responsable.IdCentroCosto.Value);
                ViewBag.CentroCostoNombre = centroCosto?.Descripcion ?? "";
            }

            return View(responsable);
        }

        // POST: Responsable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var responsable = await _context.Responsable.FindAsync(id);
            if (responsable != null)
            {
                _context.Responsable.Remove(responsable);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Responsable/ExportToExcel
        public async Task<IActionResult> ExportToExcel()
        {
            var responsables = await _context.VResponsable.ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Responsables");

                // Headers
                worksheet.Cells[1, 1].Value = "ID Responsable";
                worksheet.Cells[1, 2].Value = "Compania";
                worksheet.Cells[1, 3].Value = "Centro de Costo";
                worksheet.Cells[1, 4].Value = "Nombre";
                worksheet.Cells[1, 5].Value = "Puesto";
                worksheet.Cells[1, 6].Value = "Numero de Empleado";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Data
                int row = 2;
                foreach (var responsable in responsables)
                {
                    worksheet.Cells[row, 1].Value = responsable.IdResponsable;
                    worksheet.Cells[row, 2].Value = responsable.Compania ?? "";
                    worksheet.Cells[row, 3].Value = responsable.CentroCosto ?? "";
                    worksheet.Cells[row, 4].Value = responsable.Nombre ?? "";
                    worksheet.Cells[row, 5].Value = responsable.PuestoResponsable ?? "";
                    worksheet.Cells[row, 6].Value = responsable.NumeroEmpleado?.ToString() ?? "";
                    row++;
                }

                // Manually set column widths (AutoFit has issues on macOS)
                worksheet.Column(1).Width = 15; // ID
                worksheet.Column(2).Width = 30; // Compania
                worksheet.Column(3).Width = 30; // Centro Costo
                worksheet.Column(4).Width = 30; // Nombre
                worksheet.Column(5).Width = 30; // Puesto
                worksheet.Column(6).Width = 20; // Numero Empleado

                var fileName = "Responsables_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
                return File(package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
        }

        private void PopulateDropdowns(object? selectedCompania = null, object? selectedCentroCosto = null)
        {
            // Populate Compania dropdown
            var companias = _context.Compania
                .OrderBy(c => c.Nombre)
                .Select(c => new SelectListItem
                {
                    Value = c.IdCompania.ToString(),
                    Text = c.Nombre
                })
                .ToList();

            companias.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione una Compania --" });
            ViewBag.IdCompania = new SelectList(companias, "Value", "Text", selectedCompania);

            // Populate Centro Costo dropdown
            var centrosCosto = _context.VCentroCosto
                .OrderBy(c => c.Descripcion)
                .Select(c => new SelectListItem
                {
                    Value = c.IdCentroCosto.ToString(),
                    Text = c.Descripcion + " - " + c.Compania
                })
                .ToList();

            centrosCosto.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione un Centro de Costo --" });
            ViewBag.IdCentroCosto = new SelectList(centrosCosto, "Value", "Text", selectedCentroCosto);
        }

        private bool ResponsableExists(int id)
        {
            return _context.Responsable.Any(e => e.IdResponsable == id);
        }
    }
}
