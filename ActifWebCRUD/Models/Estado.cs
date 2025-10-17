using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("estado")]
    public class Estado
    {
        [Key]
        [Column("ID_ESTADO")]
        [Display(Name = "ID Estado")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short IdEstado { get; set; }

        [Column("NOMBRE")]
        [StringLength(60)]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [Column("ID_PAIS")]
        [Display(Name = "Pais")]
        public short? IdPais { get; set; }

        // Navigation property (manually loaded due to type mismatch)
        [NotMapped]
        public virtual Pais? Pais { get; set; }
    }
}
