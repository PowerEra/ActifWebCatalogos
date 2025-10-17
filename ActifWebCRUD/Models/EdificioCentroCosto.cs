using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("edificio_centro_costo")]
    public class EdificioCentroCosto
    {
        [Key]
        [Column("ID_EDIFICIO_CENTRO_COSTO")]
        [Display(Name = "ID Edificio Centro Costo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEdificioCentroCosto { get; set; }

        [Column("ID_COMPANIA")]
        [Required(ErrorMessage = "La compañía es obligatoria")]
        [Display(Name = "Compañía")]
        public short IdCompania { get; set; }

        [Column("ID_EDIFICIO")]
        [Required(ErrorMessage = "El edificio es obligatorio")]
        [Display(Name = "Edificio")]
        public int IdEdificio { get; set; }

        [Column("ID_CENTRO_COSTO")]
        [Required(ErrorMessage = "El centro de costo es obligatorio")]
        [Display(Name = "Centro de Costo")]
        public int IdCentroCosto { get; set; }

        // Navigation properties
        [NotMapped]
        public virtual Compania? Compania { get; set; }

        [NotMapped]
        public virtual Edificio? Edificio { get; set; }

        [NotMapped]
        public virtual CentroCosto? CentroCosto { get; set; }
    }
}
