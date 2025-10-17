using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("plantillaedificiodetalle")]
    public class PlantillaEdificioDetalle
    {
        [Key]
        [Column("IdPlantillaDetalle")]
        [Display(Name = "ID Plantilla Detalle")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPlantillaDetalle { get; set; }

        [Column("IdPlantilla")]
        [Display(Name = "Plantilla")]
        public int? IdPlantilla { get; set; }

        [Column("Id_Edificio")]
        [Display(Name = "Edificio")]
        public int? IdEdificio { get; set; }

        [Column("Id_Compania")]
        [Display(Name = "Compañía")]
        public short? IdCompania { get; set; }

        // Navigation properties (manually loaded)
        [NotMapped]
        public virtual PlantillaEdificio? PlantillaEdificio { get; set; }

        [NotMapped]
        public virtual Edificio? Edificio { get; set; }

        [NotMapped]
        public virtual Compania? Compania { get; set; }
    }
}
