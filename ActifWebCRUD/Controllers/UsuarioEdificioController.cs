using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActifWebCRUD.Data;
using ActifWebCRUD.Models;
using OfficeOpenXml;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ActifWebCRUD.Controllers
{
    public class UsuarioEdificioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UsuarioEdificioController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: UsuarioEdificio
        public async Task<IActionResult> Index()
        {
            // Query usuario_edificio table and join with related tables for display
            var usuariosEdificios = new List<UsuarioEdificio>();

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var query = @"
                    SELECT
                        ue.Id_Usuario_edificio,
                        ue.ID_USUARIO,
                        u.UserName,
                        ue.ID_COMPANIA,
                        c.NOMBRE as CompaniaNombre,
                        ue.ID_EDIFICIO,
                        e.DESCRIPCION as EdificioDescripcion
                    FROM usuario_edificio ue
                    LEFT JOIN Users u ON ue.ID_USUARIO = u.IdUser
                    LEFT JOIN compania c ON ue.ID_COMPANIA = c.ID_COMPANIA
                    LEFT JOIN edificio e ON ue.ID_EDIFICIO = e.ID_EDIFICIO
                    ORDER BY ue.Id_Usuario_edificio DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var usuarioEdificio = new UsuarioEdificio
                            {
                                IdUsuarioEdificio = reader.GetInt32(0),
                                IdUsuario = reader.GetInt32(1),
                                UserName = reader.IsDBNull(2) ? null : reader.GetString(2),
                                IdCompania = reader.GetInt16(3),
                                IdEdificio = reader.GetInt32(5)
                            };

                            // Load navigation properties
                            usuarioEdificio.Compania = new Compania
                            {
                                IdCompania = reader.GetInt16(3),
                                Nombre = reader.IsDBNull(4) ? null : reader.GetString(4)
                            };

                            usuarioEdificio.Edificio = new Edificio
                            {
                                IdEdificio = reader.GetInt16(5),
                                Descripcion = reader.IsDBNull(6) ? null : reader.GetString(6)
                            };

                            usuariosEdificios.Add(usuarioEdificio);
                        }
                    }
                }
            }

            return View(usuariosEdificios);
        }

        // GET: UsuarioEdificio/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioEdificio = await _context.UsuarioEdificio.FirstOrDefaultAsync(m => m.IdUsuarioEdificio == id);
            if (usuarioEdificio == null)
            {
                return NotFound();
            }

            // Manually load foreign key relations
            usuarioEdificio.Compania = await _context.Compania.FindAsync(usuarioEdificio.IdCompania);
            usuarioEdificio.Edificio = await _context.Edificio.FindAsync(usuarioEdificio.IdEdificio);

            // Load username from Users table
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT UserName FROM Users WHERE IdUser = @IdUser";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdUser", usuarioEdificio.IdUsuario);
                    var result = await command.ExecuteScalarAsync();
                    if (result != null)
                    {
                        usuarioEdificio.UserName = result.ToString();
                    }
                }
            }

            return View(usuarioEdificio);
        }

        // GET: UsuarioEdificio/Create
        public IActionResult Create()
        {
            PopulateUsuarioDropdown();
            PopulateCompaniaDropdown();
            PopulateEdificioDropdown();
            return View();
        }

        // POST: UsuarioEdificio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,IdCompania,IdEdificio")] UsuarioEdificio usuarioEdificio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuarioEdificio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateUsuarioDropdown(usuarioEdificio.IdUsuario);
            PopulateCompaniaDropdown(usuarioEdificio.IdCompania);
            PopulateEdificioDropdown(usuarioEdificio.IdEdificio);
            return View(usuarioEdificio);
        }

        // GET: UsuarioEdificio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioEdificio = await _context.UsuarioEdificio.FindAsync(id);
            if (usuarioEdificio == null)
            {
                return NotFound();
            }
            PopulateUsuarioDropdown(usuarioEdificio.IdUsuario);
            PopulateCompaniaDropdown(usuarioEdificio.IdCompania);
            PopulateEdificioDropdown(usuarioEdificio.IdEdificio);
            return View(usuarioEdificio);
        }

        // POST: UsuarioEdificio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuarioEdificio,IdUsuario,IdCompania,IdEdificio")] UsuarioEdificio usuarioEdificio)
        {
            if (id != usuarioEdificio.IdUsuarioEdificio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuarioEdificio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioEdificioExists(usuarioEdificio.IdUsuarioEdificio))
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
            PopulateUsuarioDropdown(usuarioEdificio.IdUsuario);
            PopulateCompaniaDropdown(usuarioEdificio.IdCompania);
            PopulateEdificioDropdown(usuarioEdificio.IdEdificio);
            return View(usuarioEdificio);
        }

        // GET: UsuarioEdificio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioEdificio = await _context.UsuarioEdificio.FirstOrDefaultAsync(m => m.IdUsuarioEdificio == id);
            if (usuarioEdificio == null)
            {
                return NotFound();
            }

            // Manually load foreign key relations
            usuarioEdificio.Compania = await _context.Compania.FindAsync(usuarioEdificio.IdCompania);
            usuarioEdificio.Edificio = await _context.Edificio.FindAsync(usuarioEdificio.IdEdificio);

            // Load username from Users table
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT UserName FROM Users WHERE IdUser = @IdUser";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdUser", usuarioEdificio.IdUsuario);
                    var result = await command.ExecuteScalarAsync();
                    if (result != null)
                    {
                        usuarioEdificio.UserName = result.ToString();
                    }
                }
            }

            return View(usuarioEdificio);
        }

        // POST: UsuarioEdificio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarioEdificio = await _context.UsuarioEdificio.FindAsync(id);
            if (usuarioEdificio != null)
            {
                _context.UsuarioEdificio.Remove(usuarioEdificio);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel using EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var usuariosEdificios = new List<UsuarioEdificio>();

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var query = @"
                    SELECT
                        ue.Id_Usuario_edificio,
                        ue.ID_USUARIO,
                        u.UserName,
                        ue.ID_COMPANIA,
                        c.NOMBRE as CompaniaNombre,
                        ue.ID_EDIFICIO,
                        e.DESCRIPCION as EdificioDescripcion
                    FROM usuario_edificio ue
                    LEFT JOIN Users u ON ue.ID_USUARIO = u.IdUser
                    LEFT JOIN compania c ON ue.ID_COMPANIA = c.ID_COMPANIA
                    LEFT JOIN edificio e ON ue.ID_EDIFICIO = e.ID_EDIFICIO
                    ORDER BY ue.Id_Usuario_edificio DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var usuarioEdificio = new UsuarioEdificio
                            {
                                IdUsuarioEdificio = reader.GetInt32(0),
                                IdUsuario = reader.GetInt32(1),
                                UserName = reader.IsDBNull(2) ? null : reader.GetString(2),
                                IdCompania = reader.GetInt16(3),
                                IdEdificio = reader.GetInt32(5)
                            };

                            // Load navigation properties
                            usuarioEdificio.Compania = new Compania
                            {
                                IdCompania = reader.GetInt16(3),
                                Nombre = reader.IsDBNull(4) ? null : reader.GetString(4)
                            };

                            usuarioEdificio.Edificio = new Edificio
                            {
                                IdEdificio = reader.GetInt16(5),
                                Descripcion = reader.IsDBNull(6) ? null : reader.GetString(6)
                            };

                            usuariosEdificios.Add(usuarioEdificio);
                        }
                    }
                }
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("UsuariosEdificios");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID Usuario Edificio";
                worksheet.Cells[1, 2].Value = "ID Usuario";
                worksheet.Cells[1, 3].Value = "Nombre Usuario";
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
                foreach (var usuarioEdificio in usuariosEdificios)
                {
                    worksheet.Cells[row, 1].Value = usuarioEdificio.IdUsuarioEdificio;
                    worksheet.Cells[row, 2].Value = usuarioEdificio.IdUsuario;
                    worksheet.Cells[row, 3].Value = usuarioEdificio.UserName;
                    worksheet.Cells[row, 4].Value = usuarioEdificio.IdCompania;
                    worksheet.Cells[row, 5].Value = usuarioEdificio.Compania?.Nombre;
                    worksheet.Cells[row, 6].Value = usuarioEdificio.IdEdificio;
                    worksheet.Cells[row, 7].Value = usuarioEdificio.Edificio?.Descripcion;
                    row++;
                }

                // Set column widths manually (AutoFitColumns not supported on non-Windows)
                for (int col = 1; col <= 7; col++)
                {
                    worksheet.Column(col).Width = 20;
                }

                var fileName = $"UsuariosEdificios_{DateTime.Now:yyyyMMdd}.xlsx";
                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        private void PopulateUsuarioDropdown(object? selectedValue = null)
        {
            var usuarios = new List<SelectListItem>();

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT IdUser, UserName FROM Users ORDER BY UserName";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new SelectListItem
                            {
                                Value = reader.GetInt32(0).ToString(),
                                Text = reader.GetString(1)
                            });
                        }
                    }
                }
            }

            usuarios.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione un Usuario --" });

            ViewBag.IdUsuario = new SelectList(usuarios, "Value", "Text", selectedValue);
        }

        private void PopulateCompaniaDropdown(object? selectedValue = null)
        {
            var companias = _context.Compania
                .OrderBy(c => c.Nombre)
                .Select(c => new SelectListItem
                {
                    Value = c.IdCompania.ToString(),
                    Text = c.Nombre
                })
                .ToList();

            companias.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione una Compañía --" });

            ViewBag.IdCompania = new SelectList(companias, "Value", "Text", selectedValue);
        }

        private void PopulateEdificioDropdown(object? selectedValue = null)
        {
            var edificios = _context.Edificio
                .OrderBy(e => e.Descripcion)
                .Select(e => new SelectListItem
                {
                    Value = e.IdEdificio.ToString(),
                    Text = e.Descripcion ?? e.IdEdificio.ToString()
                })
                .ToList();

            edificios.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione un Edificio --" });

            ViewBag.IdEdificio = new SelectList(edificios, "Value", "Text", selectedValue);
        }

        private bool UsuarioEdificioExists(int id)
        {
            return _context.UsuarioEdificio.Any(e => e.IdUsuarioEdificio == id);
        }
    }
}
