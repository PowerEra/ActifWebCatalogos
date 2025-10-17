using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("moneda")]
    public class Moneda
    {
        [Key]
        [Column("ID_MONEDA")]
        [Display(Name = "ID Moneda")]
        public short IdMoneda { get; set; }

        [Column("ID_PAIS")]
        [Display(Name = "País")]
        public int? IdPais { get; set; }

        [Column("NOMBRE")]
        [StringLength(20)]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [Column("SIMBOLO")]
        [StringLength(4)]
        [Display(Name = "Símbolo")]
        public string? Simbolo { get; set; }

        [Column("rv")]
        [Timestamp]
        public byte[]? Rv { get; set; }

        // Navigation property
        [ForeignKey("IdPais")]
        public virtual Pais? Pais { get; set; }
    }
}
