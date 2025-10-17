using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("edificio_piso")]
    public class EdificioPiso
    {
        [Key]
        [Column("ID_COMPANIA")]
        [Display(Name = "ID Compañía")]
        public short IdCompania { get; set; }

        [Key]
        [Column("ID_EDIFICIO")]
        [Display(Name = "ID Edificio")]
        public short IdEdificio { get; set; }

        [Key]
        [Column("ID_PISO")]
        [Display(Name = "ID Piso")]
        public short IdPiso { get; set; }

        // Navigation properties (manually loaded)
        [NotMapped]
        public virtual Compania? Compania { get; set; }

        [NotMapped]
        public virtual Piso? Piso { get; set; }
    }
}
