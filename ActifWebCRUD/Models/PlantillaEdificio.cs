using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("PlantillaEdificio")]
    public class PlantillaEdificio
    {
        [Key]
        [Column("IdPlantilla")]
        [Display(Name = "ID Plantilla")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPlantilla { get; set; }

        [Column("Plantilla")]
        [StringLength(120)]
        [Display(Name = "Plantilla")]
        public string? Plantilla { get; set; }

        [Column("Id_Tipo_Dep")]
        [Display(Name = "Tipo Depreciación")]
        public short? IdTipoDep { get; set; }

        [Column("TomaEdificio")]
        [Display(Name = "Toma Edificio")]
        public bool? TomaEdificio { get; set; }

        [Column("TomaCC")]
        [Display(Name = "Toma Centro Costo")]
        public bool? TomaCC { get; set; }

        [Column("Id_Compania")]
        [Display(Name = "Compañía")]
        public short? IdCompania { get; set; }

        // Navigation properties (manually loaded)
        [NotMapped]
        public virtual TipoDepreciacion? TipoDepreciacion { get; set; }

        [NotMapped]
        public virtual Compania? Compania { get; set; }
    }
}
