using ActifWebCRUD.Controllers;
using ActifWebCRUD.Models;

namespace ActifWebCRUD.Services
{
    /// <summary>
    /// Servicio de alto nivel para autenticación mediante cookies de PowerDashboard.
    /// </summary>
    public class CookieAuthenticationService
    {
        private readonly FormsAuthenticationTicketDecryptor _ticketDecryptor;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CookieAuthenticationService> _logger;

        public CookieAuthenticationService(
            FormsAuthenticationTicketDecryptor ticketDecryptor,
            IConfiguration configuration,
            ILogger<CookieAuthenticationService> logger)
        {
            _ticketDecryptor = ticketDecryptor;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene la información del usuario desde la cookie de autenticación.
        /// </summary>
        public UserInfo? GetUserFromCookie(HttpContext context)
        {
            try
            {
                // Obtiene el nombre de la cookie desde configuración (por defecto .ASPXAUTH)
                var cookieName = _configuration["FormsAuthentication:CookieName"] ?? ".ASPXAUTH";

                // Lee la cookie
                var authCookie = context.Request.Cookies[cookieName];

                if (string.IsNullOrEmpty(authCookie))
                {
                    _logger.LogDebug("Cookie de autenticación '{CookieName}' no encontrada", cookieName);
                    return null;
                }

                _logger.LogDebug("Cookie encontrada: {CookieName}", cookieName);

                // Descifra el ticket
                var userData = _ticketDecryptor.DecryptTicket(authCookie);

                if (userData == null)
                {
                    _logger.LogWarning("No se pudo descifrar el ticket de autenticación");
                    return null;
                }

                // Convierte a UserInfo
                return new UserInfo
                {
                    IdUser = userData.IdUser,
                    UserName = userData.UserName,
                    Login = userData.Login,
                    Email = userData.Email,
                    Language = userData.Language,
                    IdUserRole = userData.IdUserRole,
                    IdCompania = userData.IdCompania,
                    Inactive = userData.Inactive,
                    IsAuthenticated = true,
                    Token = userData.Token,
                    AccessToken = userData.AccessToken,
                    UUID = userData.UUID,
                    SesionUUID = userData.SesionUUID,
                    DefaultPermission = userData.DefaultPermission
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario desde la cookie");
                return null;
            }
        }

        /// <summary>
        /// Valida si el usuario está autenticado.
        /// </summary>
        public bool IsAuthenticated(HttpContext context)
        {
            var user = GetUserFromCookie(context);
            return user != null && user.IsAuthenticated && !user.Inactive;
        }
    }
}
