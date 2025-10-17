using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("usuario_edificio")]
    public class UsuarioEdificio
    {
        [Key]
        [Column("Id_Usuario_edificio")]
        [Display(Name = "ID Usuario Edificio")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuarioEdificio { get; set; }

        [Column("ID_USUARIO")]
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public int IdUsuario { get; set; }

        [Column("ID_COMPANIA")]
        [Display(Name = "Compañía")]
        [Required(ErrorMessage = "La compañía es obligatoria")]
        public short IdCompania { get; set; }

        [Column("ID_EDIFICIO")]
        [Display(Name = "Edificio")]
        [Required(ErrorMessage = "El edificio es obligatorio")]
        public int IdEdificio { get; set; }

        [Column("rv")]
        [Timestamp]
        public byte[]? Rv { get; set; }

        // Navigation properties (manually loaded)
        [NotMapped]
        public virtual Compania? Compania { get; set; }

        [NotMapped]
        public virtual Edificio? Edificio { get; set; }

        // Note: Users table reference - manually loaded
        [NotMapped]
        public string? UserName { get; set; }
    }
}
