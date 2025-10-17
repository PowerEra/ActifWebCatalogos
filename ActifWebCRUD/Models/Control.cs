using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("control")]
    public class Control
    {
        [Key]
        [Column("ID_CONTROL")]
        [Display(Name = "ID Control")]
        public short IdControl { get; set; }

        [Column("TITULO")]
        [StringLength(50)]
        [Display(Name = "Titulo")]
        public string? Titulo { get; set; }

        [Column("CONTENIDO")]
        [StringLength(200)]
        [Display(Name = "Contenido")]
        public string? Contenido { get; set; }

        [Column("FLG_USAR")]
        [StringLength(2)]
        [Display(Name = "Flag Usar")]
        public string? FlgUsar { get; set; }

        [Column("TIPO")]
        [StringLength(2)]
        [Display(Name = "Tipo")]
        public string? Tipo { get; set; }

        [Column("LONGITUD")]
        [Display(Name = "Longitud")]
        public short? Longitud { get; set; }

        [Column("DECIMALES")]
        [Display(Name = "Decimales")]
        public short? Decimales { get; set; }

        [Column("RV")]
        [Timestamp]
        [Display(Name = "RV")]
        public byte[]? Rv { get; set; }
    }
}
