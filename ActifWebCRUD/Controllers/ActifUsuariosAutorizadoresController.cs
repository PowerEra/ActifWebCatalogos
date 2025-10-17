using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;
using Microsoft.Data.SqlClient;

namespace ActifWebCRUD.Controllers
{
    public class ActifUsuariosAutorizadoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public ActifUsuariosAutorizadoresController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: ActifUsuariosAutorizadores
        public async Task<IActionResult> Index()
        {
            var autorizadores = await _context.ActifUsuariosAutorizadores.ToListAsync();

            // Load related data using dictionaries
            var userNames = await _context.Database.SqlQueryRaw<UserInfo>("SELECT IdUser, UserName FROM Users")
                .ToDictionaryAsync(u => u.IdUser, u => u.UserName ?? "");
            var companias = await _context.Compania.ToDictionaryAsync(c => c.IdCompania, c => c.Nombre);
            var edificios = await _context.Edificio.ToDictionaryAsync(e => e.IdEdificio, e => e.Descripcion ?? "");

            // Manually populate navigation properties
            foreach (var autorizador in autorizadores)
            {
                autorizador.UserName = userNames.ContainsKey(autorizador.IdUsuario) ? userNames[autorizador.IdUsuario] : "";
                autorizador.CompaniaName = companias.ContainsKey(autorizador.IdCompania) ? companias[autorizador.IdCompania] : "";
                int edificioId = autorizador.IdEdificio;
                autorizador.EdificioDesc = edificios.ContainsKey(edificioId) ? edificios[edificioId] : "";
            }

            return View(autorizadores);
        }

        // GET: ActifUsuariosAutorizadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autorizador = await _context.ActifUsuariosAutorizadores
                .FirstOrDefaultAsync(m => m.IdUsuarioAutorizador == id);

            if (autorizador == null)
            {
                return NotFound();
            }

            // Load related data
            var userName = await _context.Database.SqlQueryRaw<UserInfo>($"SELECT IdUser, UserName FROM Users WHERE IdUser = {autorizador.IdUsuario}")
                .FirstOrDefaultAsync();
            var compania = await _context.Compania.FindAsync(autorizador.IdCompania);
            int edificioId = autorizador.IdEdificio;
            var edificio = await _context.Edificio.FindAsync(edificioId);

            autorizador.UserName = userName?.UserName ?? "";
            autorizador.CompaniaName = compania?.Nombre ?? "";
            autorizador.EdificioDesc = edificio?.Descripcion ?? "";

            return View(autorizador);
        }

        // GET: ActifUsuariosAutorizadores/Create
        public async Task<IActionResult> Create()
        {
            var users = await _context.Database.SqlQueryRaw<UserInfo>("SELECT IdUser, UserName FROM Users WHERE Inactive IS NULL OR Inactive = 0")
                .ToListAsync();
            ViewData["IdUsuario"] = new SelectList(users.OrderBy(u => u.UserName), "IdUser", "UserName");
            ViewData["IdCompania"] = new SelectList(_context.Compania.OrderBy(c => c.Nombre), "IdCompania", "Nombre");

            // Get edificios and manually create list items
            var edificios = _context.Edificio.OrderBy(e => e.Descripcion).ToList();
            var edificioItems = edificios.Select(e => new SelectListItem
            {
                Value = e.IdEdificio.ToString(),
                Text = e.Descripcion ?? e.IdEdificio.ToString()
            }).ToList();
            ViewData["IdEdificio"] = edificioItems;

            return View();
        }

        // POST: ActifUsuariosAutorizadores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,IdCompania,IdEdificio")] ActifUsuariosAutorizadores autorizador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autorizador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var users = await _context.Database.SqlQueryRaw<UserInfo>("SELECT IdUser, UserName FROM Users WHERE Inactive IS NULL OR Inactive = 0")
                .ToListAsync();
            ViewData["IdUsuario"] = new SelectList(users.OrderBy(u => u.UserName), "IdUser", "UserName", autorizador.IdUsuario);
            ViewData["IdCompania"] = new SelectList(_context.Compania.OrderBy(c => c.Nombre), "IdCompania", "Nombre", autorizador.IdCompania);

            var edificios = _context.Edificio.OrderBy(e => e.Descripcion).ToList();
            var edificioItems = edificios.Select(e => new SelectListItem
            {
                Value = e.IdEdificio.ToString(),
                Text = e.Descripcion ?? e.IdEdificio.ToString(),
                Selected = e.IdEdificio == autorizador.IdEdificio
            }).ToList();
            ViewData["IdEdificio"] = edificioItems;

            return View(autorizador);
        }

        // GET: ActifUsuariosAutorizadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autorizador = await _context.ActifUsuariosAutorizadores.FindAsync(id);
            if (autorizador == null)
            {
                return NotFound();
            }

            var users = await _context.Database.SqlQueryRaw<UserInfo>("SELECT IdUser, UserName FROM Users WHERE Inactive IS NULL OR Inactive = 0")
                .ToListAsync();
            ViewData["IdUsuario"] = new SelectList(users.OrderBy(u => u.UserName), "IdUser", "UserName", autorizador.IdUsuario);
            ViewData["IdCompania"] = new SelectList(_context.Compania.OrderBy(c => c.Nombre), "IdCompania", "Nombre", autorizador.IdCompania);

            var edificios = _context.Edificio.OrderBy(e => e.Descripcion).ToList();
            var edificioItems = edificios.Select(e => new SelectListItem
            {
                Value = e.IdEdificio.ToString(),
                Text = e.Descripcion ?? e.IdEdificio.ToString(),
                Selected = e.IdEdificio == autorizador.IdEdificio
            }).ToList();
            ViewData["IdEdificio"] = edificioItems;

            return View(autorizador);
        }

        // POST: ActifUsuariosAutorizadores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuarioAutorizador,IdUsuario,IdCompania,IdEdificio")] ActifUsuariosAutorizadores autorizador)
        {
            if (id != autorizador.IdUsuarioAutorizador)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autorizador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActifUsuariosAutorizadoresExists(autorizador.IdUsuarioAutorizador))
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

            var users = await _context.Database.SqlQueryRaw<UserInfo>("SELECT IdUser, UserName FROM Users WHERE Inactive IS NULL OR Inactive = 0")
                .ToListAsync();
            ViewData["IdUsuario"] = new SelectList(users.OrderBy(u => u.UserName), "IdUser", "UserName", autorizador.IdUsuario);
            ViewData["IdCompania"] = new SelectList(_context.Compania.OrderBy(c => c.Nombre), "IdCompania", "Nombre", autorizador.IdCompania);

            var edificios = _context.Edificio.OrderBy(e => e.Descripcion).ToList();
            var edificioItems = edificios.Select(e => new SelectListItem
            {
                Value = e.IdEdificio.ToString(),
                Text = e.Descripcion ?? e.IdEdificio.ToString(),
                Selected = e.IdEdificio == autorizador.IdEdificio
            }).ToList();
            ViewData["IdEdificio"] = edificioItems;

            return View(autorizador);
        }

        // GET: ActifUsuariosAutorizadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autorizador = await _context.ActifUsuariosAutorizadores
                .FirstOrDefaultAsync(m => m.IdUsuarioAutorizador == id);

            if (autorizador == null)
            {
                return NotFound();
            }

            // Load related data
            var userName = await _context.Database.SqlQueryRaw<UserInfo>($"SELECT IdUser, UserName FROM Users WHERE IdUser = {autorizador.IdUsuario}")
                .FirstOrDefaultAsync();
            var compania = await _context.Compania.FindAsync(autorizador.IdCompania);
            int edificioId = autorizador.IdEdificio;
            var edificio = await _context.Edificio.FindAsync(edificioId);

            autorizador.UserName = userName?.UserName ?? "";
            autorizador.CompaniaName = compania?.Nombre ?? "";
            autorizador.EdificioDesc = edificio?.Descripcion ?? "";

            return View(autorizador);
        }

        // POST: ActifUsuariosAutorizadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autorizador = await _context.ActifUsuariosAutorizadores.FindAsync(id);
            if (autorizador != null)
            {
                _context.ActifUsuariosAutorizadores.Remove(autorizador);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var autorizadores = await _context.ActifUsuariosAutorizadores.ToListAsync();

            // Pre-load all related data
            var userNames = await _context.Database.SqlQueryRaw<UserInfo>("SELECT IdUser, UserName FROM Users")
                .ToDictionaryAsync(u => u.IdUser, u => u.UserName ?? "");
            var companias = await _context.Compania.ToDictionaryAsync(c => c.IdCompania, c => c.Nombre);
            var edificios = await _context.Edificio.ToDictionaryAsync(e => e.IdEdificio, e => e.Descripcion ?? "");

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Usuarios Autorizadores");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID Autorizador";
                worksheet.Cells[1, 2].Value = "ID Usuario";
                worksheet.Cells[1, 3].Value = "Usuario";
                worksheet.Cells[1, 4].Value = "ID Compañía";
                worksheet.Cells[1, 5].Value = "Compañía";
                worksheet.Cells[1, 6].Value = "ID Edificio";
                worksheet.Cells[1, 7].Value = "Edificio";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 7])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var autorizador in autorizadores)
                {
                    worksheet.Cells[row, 1].Value = autorizador.IdUsuarioAutorizador;
                    worksheet.Cells[row, 2].Value = autorizador.IdUsuario;
                    worksheet.Cells[row, 3].Value = userNames.ContainsKey(autorizador.IdUsuario) ? userNames[autorizador.IdUsuario] : "";
                    worksheet.Cells[row, 4].Value = autorizador.IdCompania;
                    worksheet.Cells[row, 5].Value = companias.ContainsKey(autorizador.IdCompania) ? companias[autorizador.IdCompania] : "";
                    worksheet.Cells[row, 6].Value = autorizador.IdEdificio;

                    // Cast short to int for edificio lookup
                    int edificioKey = autorizador.IdEdificio;
                    worksheet.Cells[row, 7].Value = edificios.ContainsKey(edificioKey) ? edificios[edificioKey] : "";

                    row++;
                }

                // Set column widths
                worksheet.Column(1).Width = 15;
                worksheet.Column(2).Width = 15;
                worksheet.Column(3).Width = 30;
                worksheet.Column(4).Width = 15;
                worksheet.Column(5).Width = 30;
                worksheet.Column(6).Width = 15;
                worksheet.Column(7).Width = 30;

                // Return the Excel file
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"ActifUsuariosAutorizadores_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
        }

        private bool ActifUsuariosAutorizadoresExists(int id)
        {
            return _context.ActifUsuariosAutorizadores.Any(e => e.IdUsuarioAutorizador == id);
        }
    }

    // Helper class for SQL query results
    public class UserInfo
    {
        public int IdUser { get; set; }
        public string? UserName { get; set; }
    }
}
