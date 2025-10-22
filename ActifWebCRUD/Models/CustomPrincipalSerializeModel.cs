namespace ActifWebCRUD.Models
{
    /// <summary>
    /// Modelo que representa los datos del usuario almacenados en la cookie de autenticación.
    /// Este modelo debe coincidir con el CustomPrincipalSerializeModel usado en PowerDashboard.
    /// </summary>
    public class CustomPrincipalSerializeModel
    {
        public int IdUser { get; set; }
        public string? UserName { get; set; }
        public string? UUID { get; set; }
        public string? SesionUUID { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Language { get; set; }
        public bool Inactive { get; set; }
        public string? Token { get; set; }
        public string? AccessToken { get; set; }
        public string? URLMain { get; set; }
        public string? Phone { get; set; }
        public string? Connection { get; set; }
        public int IdUserRole { get; set; }
        public bool IdGrantorUser { get; set; }  // Nota: En el JSON es bool, no int
        public int IdCompania { get; set; }
        public string? RFC { get; set; }
        public string? Nombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? UserServer { get; set; }
        public string? UserDatabase { get; set; }
        public string? ForcedFilterField { get; set; }
        public string? ForcedFilterValue { get; set; }
        public bool DefaultPermission { get; set; }
    }
}
