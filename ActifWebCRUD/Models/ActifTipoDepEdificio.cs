using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("actif_tipodep_edificio")]
    public class ActifTipoDepEdificio
    {
        [Key]
        [Column("Idx")]
        [Display(Name = "ID")]
        public int Idx { get; set; }

        [Column("ID_TIPO_DEP")]
        [Display(Name = "ID Tipo Depreciación")]
        public short? IdTipoDep { get; set; }

        [Column("ID_EDIFICIO")]
        [Display(Name = "ID Edificio")]
        public int? IdEdificio { get; set; }

        [Column("FECHA_CAPTURA")]
        [Display(Name = "Fecha Captura")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaCaptura { get; set; }

        // Navigation properties (manually loaded)
        [NotMapped]
        public virtual TipoDepreciacion? TipoDepreciacion { get; set; }

        [NotMapped]
        public virtual Edificio? Edificio { get; set; }
    }
}
