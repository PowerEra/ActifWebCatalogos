namespace ActifWebCRUD.Models
{
    /// <summary>
    /// Modelo simplificado de información del usuario para retornar en los endpoints.
    /// </summary>
    public class UserInfo
    {
        public int IdUser { get; set; }
        public string? UserName { get; set; }
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? Language { get; set; }
        public int IdUserRole { get; set; }
        public int IdCompania { get; set; }
        public bool Inactive { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? Token { get; set; }
        public string? AccessToken { get; set; }
        public string? UUID { get; set; }
        public string? SesionUUID { get; set; }
        public bool DefaultPermission { get; set; }
    }
}
