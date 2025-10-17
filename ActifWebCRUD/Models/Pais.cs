using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("pais")]
    public class Pais
    {
        [Key]
        [Column("ID_PAIS")]
        [Display(Name = "ID Pais")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPais { get; set; }

        [Column("NOMBRE")]
        [StringLength(20)]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [Column("rv")]
        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "RV")]
        public byte[]? Rv { get; set; }
    }
}
