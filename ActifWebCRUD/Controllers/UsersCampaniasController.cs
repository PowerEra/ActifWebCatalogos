using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;

namespace ActifWebCRUD.Controllers
{
    public class UsersCampaniasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersCampaniasController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: UsersCampanias
        public async Task<IActionResult> Index()
        {
            // Use vUsers_Campanias view for Index display
            var query = @"
                SELECT
                    uc.ID_USER_COMPANIA,
                    u.UserName,
                    c.NOMBRE as CompaniaNombre,
                    uc.IDUSER,
                    uc.IDCOMPANIA
                FROM users_campanias uc
                LEFT JOIN Users u ON uc.IDUSER = u.IdUser
                LEFT JOIN compania c ON uc.IDCOMPANIA = c.ID_COMPANIA
                ORDER BY uc.ID_USER_COMPANIA";

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var usersCampanias = new List<dynamic>();

            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            usersCampanias.Add(new
                            {
                                IdUserCompania = reader.GetInt32(0),
                                UserName = reader.IsDBNull(1) ? null : reader.GetString(1),
                                CompaniaNombre = reader.IsDBNull(2) ? null : reader.GetString(2),
                                IdUser = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                IdCompania = reader.IsDBNull(4) ? (short?)null : reader.GetInt16(4)
                            });
                        }
                    }
                }
            }

            return View(usersCampanias);
        }

        // GET: UsersCampanias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersCampanias = await _context.UsersCampanias
                .FirstOrDefaultAsync(m => m.IdUserCompania == id);

            if (usersCampanias == null)
            {
                return NotFound();
            }

            // Load User manually for display
            if (usersCampanias.IdUser != null)
            {
                var user = await _context.User.FindAsync(usersCampanias.IdUser.Value);
                usersCampanias.User = user;
            }

            // Load Compania manually for display
            if (usersCampanias.IdCompania != null)
            {
                var compania = await _context.Compania.FindAsync(usersCampanias.IdCompania.Value);
                usersCampanias.Compania = compania;
            }

            return View(usersCampanias);
        }

        // GET: UsersCampanias/Create
        public IActionResult Create()
        {
            ViewBag.IdUser = new SelectList(_context.User, "IdUser", "UserName");
            ViewBag.IdCompania = new SelectList(_context.Compania, "IdCompania", "Nombre");
            return View();
        }

        // POST: UsersCampanias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUserCompania,IdUser,IdCompania")] UsersCampanias usersCampanias)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usersCampanias);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.IdUser = new SelectList(_context.User, "IdUser", "UserName", usersCampanias.IdUser);
            ViewBag.IdCompania = new SelectList(_context.Compania, "IdCompania", "Nombre", usersCampanias.IdCompania);
            return View(usersCampanias);
        }

        // GET: UsersCampanias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersCampanias = await _context.UsersCampanias.FindAsync(id);
            if (usersCampanias == null)
            {
                return NotFound();
            }
            ViewBag.IdUser = new SelectList(_context.User, "IdUser", "UserName", usersCampanias.IdUser);
            ViewBag.IdCompania = new SelectList(_context.Compania, "IdCompania", "Nombre", usersCampanias.IdCompania);
            return View(usersCampanias);
        }

        // POST: UsersCampanias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUserCompania,IdUser,IdCompania")] UsersCampanias usersCampanias)
        {
            if (id != usersCampanias.IdUserCompania)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the original rv value
                    var original = await _context.UsersCampanias.AsNoTracking().FirstOrDefaultAsync(c => c.IdUserCompania == id);
                    if (original != null)
                    {
                        usersCampanias.Rv = original.Rv;
                    }

                    _context.Update(usersCampanias);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersCampaniasExists(usersCampanias.IdUserCompania))
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
            ViewBag.IdUser = new SelectList(_context.User, "IdUser", "UserName", usersCampanias.IdUser);
            ViewBag.IdCompania = new SelectList(_context.Compania, "IdCompania", "Nombre", usersCampanias.IdCompania);
            return View(usersCampanias);
        }

        // GET: UsersCampanias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersCampanias = await _context.UsersCampanias
                .FirstOrDefaultAsync(m => m.IdUserCompania == id);

            if (usersCampanias == null)
            {
                return NotFound();
            }

            // Load User manually for display
            if (usersCampanias.IdUser != null)
            {
                var user = await _context.User.FindAsync(usersCampanias.IdUser.Value);
                usersCampanias.User = user;
            }

            // Load Compania manually for display
            if (usersCampanias.IdCompania != null)
            {
                var compania = await _context.Compania.FindAsync(usersCampanias.IdCompania.Value);
                usersCampanias.Compania = compania;
            }

            return View(usersCampanias);
        }

        // POST: UsersCampanias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usersCampanias = await _context.UsersCampanias.FindAsync(id);
            if (usersCampanias != null)
            {
                _context.UsersCampanias.Remove(usersCampanias);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            // Use vUsers_Campanias view for export
            var query = @"
                SELECT
                    uc.ID_USER_COMPANIA,
                    u.UserName,
                    c.NOMBRE as CompaniaNombre,
                    uc.IDUSER,
                    uc.IDCOMPANIA
                FROM users_campanias uc
                LEFT JOIN Users u ON uc.IDUSER = u.IdUser
                LEFT JOIN compania c ON uc.IDCOMPANIA = c.ID_COMPANIA
                ORDER BY uc.ID_USER_COMPANIA";

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var usersCampanias = new List<dynamic>();

            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            usersCampanias.Add(new
                            {
                                IdUserCompania = reader.GetInt32(0),
                                UserName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                CompaniaNombre = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                IdUser = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                IdCompania = reader.IsDBNull(4) ? (short?)null : reader.GetInt16(4)
                            });
                        }
                    }
                }
            }

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Usuarios Companias");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Usuario";
                worksheet.Cells[1, 3].Value = "Compania";
                worksheet.Cells[1, 4].Value = "ID Usuario";
                worksheet.Cells[1, 5].Value = "ID Compania";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var uc in usersCampanias)
                {
                    worksheet.Cells[row, 1].Value = uc.IdUserCompania;
                    worksheet.Cells[row, 2].Value = uc.UserName;
                    worksheet.Cells[row, 3].Value = uc.CompaniaNombre;
                    worksheet.Cells[row, 4].Value = uc.IdUser;
                    worksheet.Cells[row, 5].Value = uc.IdCompania;
                    row++;
                }

                // Set column widths
                worksheet.Column(1).Width = 15;
                worksheet.Column(2).Width = 30;
                worksheet.Column(3).Width = 30;
                worksheet.Column(4).Width = 15;
                worksheet.Column(5).Width = 15;

                // Return the Excel file
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"UsersCampanias_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
        }

        private bool UsersCampaniasExists(int id)
        {
            return _context.UsersCampanias.Any(e => e.IdUserCompania == id);
        }
    }
}
