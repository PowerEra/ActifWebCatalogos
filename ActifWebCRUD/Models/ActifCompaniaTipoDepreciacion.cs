using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("actif_compania_tipo_depreciacion")]
    public class ActifCompaniaTipoDepreciacion
    {
        [Key]
        [Column("ID_COMPANIA_DEPRECIACION")]
        [Display(Name = "ID Compania Depreciacion")]
        public int IdCompaniaDepreciacion { get; set; }

        [Required]
        [Column("ID_COMPANIA")]
        [Display(Name = "Compania")]
        public short IdCompania { get; set; }

        [Required]
        [Column("ID_TIPO_DEP")]
        [Display(Name = "Tipo Depreciacion")]
        public short IdTipoDep { get; set; }

        [Column("RV")]
        [Timestamp]
        public byte[] Rv { get; set; } = null!;

        // Navigation properties
        [ForeignKey("IdCompania")]
        public virtual Compania? Compania { get; set; }

        [ForeignKey("IdTipoDep")]
        public virtual TipoDepreciacion? TipoDepreciacion { get; set; }
    }
}
