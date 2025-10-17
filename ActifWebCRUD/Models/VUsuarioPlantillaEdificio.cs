using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("vUsuario_PlantillaEdificio")]
    public class VUsuarioPlantillaEdificio
    {
        [Key]
        [Column("idx")]
        public int Idx { get; set; }

        [Column("idplantilla")]
        public int? IdPlantilla { get; set; }

        [Column("UserName")]
        public string? UserName { get; set; }

        [Column("Plantilla")]
        public string? Plantilla { get; set; }
    }
}
