using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("usuario_plantillaedificio")]
    public class UsuarioPlantillaEdificio
    {
        [Key]
        [Column("idx")]
        public int Idx { get; set; }

        [Column("IdUser")]
        public int? IdUser { get; set; }

        [Column("idplantilla")]
        public int? IdPlantilla { get; set; }

        // Navigation properties
        [NotMapped]
        public string? UserName { get; set; }

        [NotMapped]
        public string? Plantilla { get; set; }
    }
}
