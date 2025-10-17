using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class UsuarioPlantillaEdificioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UsuarioPlantillaEdificioController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: UsuarioPlantillaEdificio
        public async Task<IActionResult> Index()
        {
            var usuariosPlantilla = await _context.VUsuarioPlantillaEdificio.ToListAsync();
            return View(usuariosPlantilla);
        }

        // GET: UsuarioPlantillaEdificio/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioPlantilla = await _context.VUsuarioPlantillaEdificio.FirstOrDefaultAsync(m => m.Idx == id);
            if (usuarioPlantilla == null)
            {
                return NotFound();
            }

            return View(usuarioPlantilla);
        }

        // GET: UsuarioPlantillaEdificio/Create
        public IActionResult Create()
        {
            PopulateUsersDropdown();
            PopulatePlantillaEdificioDropdown();
            return View();
        }

        // POST: UsuarioPlantillaEdificio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUser,IdPlantilla")] UsuarioPlantillaEdificio usuarioPlantilla)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuarioPlantilla);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateUsersDropdown(usuarioPlantilla.IdUser);
            PopulatePlantillaEdificioDropdown(usuarioPlantilla.IdPlantilla);
            return View(usuarioPlantilla);
        }

        // GET: UsuarioPlantillaEdificio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioPlantilla = await _context.UsuarioPlantillaEdificio.FindAsync(id);
            if (usuarioPlantilla == null)
            {
                return NotFound();
            }
            PopulateUsersDropdown(usuarioPlantilla.IdUser);
            PopulatePlantillaEdificioDropdown(usuarioPlantilla.IdPlantilla);
            return View(usuarioPlantilla);
        }

        // POST: UsuarioPlantillaEdificio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idx,IdUser,IdPlantilla")] UsuarioPlantillaEdificio usuarioPlantilla)
        {
            if (id != usuarioPlantilla.Idx)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuarioPlantilla);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioPlantillaEdificioExists(usuarioPlantilla.Idx))
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
            PopulateUsersDropdown(usuarioPlantilla.IdUser);
            PopulatePlantillaEdificioDropdown(usuarioPlantilla.IdPlantilla);
            return View(usuarioPlantilla);
        }

        // GET: UsuarioPlantillaEdificio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioPlantilla = await _context.VUsuarioPlantillaEdificio.FirstOrDefaultAsync(m => m.Idx == id);
            if (usuarioPlantilla == null)
            {
                return NotFound();
            }

            return View(usuarioPlantilla);
        }

        // POST: UsuarioPlantillaEdificio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarioPlantilla = await _context.UsuarioPlantillaEdificio.FindAsync(id);
            if (usuarioPlantilla != null)
            {
                _context.UsuarioPlantillaEdificio.Remove(usuarioPlantilla);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var usuariosPlantilla = await _context.VUsuarioPlantillaEdificio.ToListAsync();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("UsuarioPlantillaEdificio");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Usuario";
                worksheet.Cells[1, 3].Value = "ID Plantilla";
                worksheet.Cells[1, 4].Value = "Plantilla";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 4])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var item in usuariosPlantilla)
                {
                    worksheet.Cells[row, 1].Value = item.Idx;
                    worksheet.Cells[row, 2].Value = item.UserName;
                    worksheet.Cells[row, 3].Value = item.IdPlantilla;
                    worksheet.Cells[row, 4].Value = item.Plantilla;
                    row++;
                }

                // Set column widths manually (AutoFitColumns not supported on non-Windows)
                for (int col = 1; col <= 4; col++)
                {
                    worksheet.Column(col).Width = 25;
                }

                var fileName = $"UsuarioPlantillaEdificio_{DateTime.Now:yyyyMMdd}.xlsx";
                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        private void PopulateUsersDropdown(object? selectedValue = null)
        {
            var users = _context.User
                .Where(u => u.Inactive == false || u.Inactive == null)
                .OrderBy(u => u.UserName)
                .Select(u => new SelectListItem
                {
                    Value = u.IdUser.ToString(),
                    Text = u.UserName ?? ""
                })
                .ToList();

            users.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione un Usuario --" });

            ViewBag.IdUser = new SelectList(users, "Value", "Text", selectedValue);
        }

        private void PopulatePlantillaEdificioDropdown(object? selectedValue = null)
        {
            var plantillas = _context.PlantillaEdificio
                .OrderBy(p => p.Plantilla)
                .Select(p => new SelectListItem
                {
                    Value = p.IdPlantilla.ToString(),
                    Text = p.Plantilla
                })
                .ToList();

            plantillas.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione una Plantilla --" });

            ViewBag.IdPlantilla = new SelectList(plantillas, "Value", "Text", selectedValue);
        }

        private bool UsuarioPlantillaEdificioExists(int id)
        {
            return _context.UsuarioPlantillaEdificio.Any(e => e.Idx == id);
        }
    }
}
