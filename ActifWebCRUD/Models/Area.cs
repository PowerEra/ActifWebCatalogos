using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("area")]
    public class Area
    {
        [Key]
        [Column("ID_AREA")]
        [Display(Name = "ID Area")]
        public int IdArea { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(40)]
        [Display(Name = "Descripcion")]
        public string? Descripcion { get; set; }

        [Column("CTA1")]
        [StringLength(6)]
        [Display(Name = "Cuenta 1")]
        public string? Cta1 { get; set; }

        [Column("CTA2")]
        [StringLength(6)]
        [Display(Name = "Cuenta 2")]
        public string? Cta2 { get; set; }

        [Column("CTA3")]
        [StringLength(6)]
        [Display(Name = "Cuenta 3")]
        public string? Cta3 { get; set; }

        [Column("CTA4")]
        [StringLength(6)]
        [Display(Name = "Cuenta 4")]
        public string? Cta4 { get; set; }

        [Column("CTA5")]
        [StringLength(6)]
        [Display(Name = "Cuenta 5")]
        public string? Cta5 { get; set; }

        [Column("CTA6")]
        [StringLength(6)]
        [Display(Name = "Cuenta 6")]
        public string? Cta6 { get; set; }

        [Column("ID_EDIFICIO")]
        [Display(Name = "ID Edificio")]
        public int? IdEdificio { get; set; }

        [Column("ID_PISO")]
        [Display(Name = "ID Piso")]
        public int? IdPiso { get; set; }

        [Column("rv")]
        [Timestamp]
        [Display(Name = "RV")]
        public byte[]? Rv { get; set; }

        [Column("Id_Area_Orig")]
        [Display(Name = "ID Area Origen")]
        public int? IdAreaOrig { get; set; }

        [Column("Id_Compania")]
        [Display(Name = "ID Compania")]
        public int? IdCompania { get; set; }
    }
}
