using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("vEstado")]
    public class VEstado
    {
        [Key]
        [Column("ID_ESTADO")]
        [Display(Name = "ID Estado")]
        public short IdEstado { get; set; }

        [Column("PAIS")]
        [StringLength(20)]
        [Display(Name = "Pais")]
        public string? Pais { get; set; }

        [Column("NOMBRE")]
        [StringLength(60)]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }
    }
}
