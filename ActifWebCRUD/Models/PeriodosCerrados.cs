using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("PeriodosCerrados")]
    public class PeriodosCerrados
    {
        [Key]
        [Column("Id_Cierre_Compania")]
        [Display(Name = "ID Cierre Compania")]
        public int IdCierreCompania { get; set; }

        [Column("Anio")]
        [Required(ErrorMessage = "El año es obligatorio")]
        [Display(Name = "Año")]
        public int Anio { get; set; }

        [Column("Mes")]
        [Required(ErrorMessage = "El mes es obligatorio")]
        [Display(Name = "Mes")]
        [Range(1, 12, ErrorMessage = "El mes debe estar entre 1 y 12")]
        public int Mes { get; set; }

        [Column("ID_Compania")]
        [Required(ErrorMessage = "La compañía es obligatoria")]
        [Display(Name = "ID Compania")]
        public short IdCompania { get; set; }

        [Column("FECHA_CIERRE")]
        [Display(Name = "Fecha Cierre")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaCierre { get; set; }

        [Column("ID_USUARIO_CIERRE")]
        [Display(Name = "ID Usuario Cierre")]
        public int? IdUsuarioCierre { get; set; }

        [Column("ID_TIPO_DEP")]
        [Display(Name = "ID Tipo Depreciacion")]
        public short? IdTipoDep { get; set; }

        // Navigation properties
        [ForeignKey("IdCompania")]
        public virtual Compania? Compania { get; set; }

        [ForeignKey("IdTipoDep")]
        public virtual TipoDepreciacion? TipoDepreciacion { get; set; }
    }
}
