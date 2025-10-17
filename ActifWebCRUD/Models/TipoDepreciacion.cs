using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("tipo_depreciacion")]
    public class TipoDepreciacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_TIPO_DEP")]
        [Display(Name = "ID Tipo Depreciacion")]
        public short IdTipoDep { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(20)]
        [Display(Name = "Descripcion")]
        public string? Descripcion { get; set; }

        [Column("MES_INI")]
        [Display(Name = "Mes Inicial")]
        public short? MesIni { get; set; }

        [Column("AplicaFiscal")]
        [Display(Name = "Aplica Fiscal")]
        public int? AplicaFiscal { get; set; }
    }
}
