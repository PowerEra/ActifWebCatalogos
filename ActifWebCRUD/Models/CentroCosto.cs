using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("centro_costo")]
    public class CentroCosto
    {
        [Key]
        [Column("ID_CENTRO_COSTO")]
        [Display(Name = "ID Centro Costo")]
        public int IdCentroCosto { get; set; }

        [Column("ID_COMPANIA")]
        [Required(ErrorMessage = "La compañía es obligatoria")]
        [Display(Name = "Compañía")]
        public short IdCompania { get; set; }

        [Column("CODIGO")]
        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(50)]
        [Display(Name = "Código")]
        public string? Codigo { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(255)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Column("RESPONSABLE")]
        [StringLength(100)]
        [Display(Name = "Responsable")]
        public string? Responsable { get; set; }

        [Column("STATUS")]
        [Required(ErrorMessage = "El status es obligatorio")]
        [Display(Name = "Status")]
        public short Status { get; set; }

        [Column("CTA1")]
        [StringLength(50)]
        [Display(Name = "Cuenta 1")]
        public string? Cta1 { get; set; }

        [Column("CTA2")]
        [StringLength(50)]
        [Display(Name = "Cuenta 2")]
        public string? Cta2 { get; set; }

        [Column("CTA3")]
        [StringLength(50)]
        [Display(Name = "Cuenta 3")]
        public string? Cta3 { get; set; }

        [Column("CTA4")]
        [StringLength(50)]
        [Display(Name = "Cuenta 4")]
        public string? Cta4 { get; set; }

        [Column("CTA5")]
        [StringLength(50)]
        [Display(Name = "Cuenta 5")]
        public string? Cta5 { get; set; }

        [Column("CTA6")]
        [StringLength(50)]
        [Display(Name = "Cuenta 6")]
        public string? Cta6 { get; set; }

        [Column("PRORRA")]
        [Display(Name = "Prorrateo")]
        public short? Prorra { get; set; }

        [Column("TRANS_ENTRA")]
        [Display(Name = "Transferencias Entrada")]
        public double? TransEntra { get; set; }

        [Column("TRANS_SALE")]
        [Display(Name = "Transferencias Salida")]
        public double? TransSale { get; set; }

        [Column("BAJAS")]
        [Display(Name = "Bajas")]
        public double? Bajas { get; set; }

        [Column("ALTAS")]
        [Display(Name = "Altas")]
        public double? Altas { get; set; }

        [Column("CTA11")]
        [StringLength(50)]
        [Display(Name = "Cuenta 11")]
        public string? Cta11 { get; set; }

        [Column("CTA12")]
        [StringLength(50)]
        [Display(Name = "Cuenta 12")]
        public string? Cta12 { get; set; }

        [Column("CTA13")]
        [StringLength(50)]
        [Display(Name = "Cuenta 13")]
        public string? Cta13 { get; set; }

        [Column("CTA14")]
        [StringLength(50)]
        [Display(Name = "Cuenta 14")]
        public string? Cta14 { get; set; }

        [Column("CTA15")]
        [StringLength(50)]
        [Display(Name = "Cuenta 15")]
        public string? Cta15 { get; set; }

        [Column("CTA16")]
        [StringLength(50)]
        [Display(Name = "Cuenta 16")]
        public string? Cta16 { get; set; }

        [Column("rv")]
        [Timestamp]
        [Display(Name = "RV")]
        public byte[]? Rv { get; set; }

        // Navigation property
        [ForeignKey("IdCompania")]
        public virtual Compania? Compania { get; set; }
    }
}
