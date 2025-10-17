using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("actif_usuarios_autorizadores")]
    public class ActifUsuariosAutorizadores
    {
        [Key]
        [Column("ID_USUARIO_AUTORIZADOR")]
        public int IdUsuarioAutorizador { get; set; }

        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [Column("ID_COMPANIA")]
        public short IdCompania { get; set; }

        [Column("ID_EDIFICIO")]
        public int IdEdificio { get; set; }

        // Navigation properties - loaded manually to avoid conflicts
        [NotMapped]
        public string? UserName { get; set; }

        [NotMapped]
        public string? CompaniaName { get; set; }

        [NotMapped]
        public string? EdificioDesc { get; set; }
    }
}
