using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("piso")]
    public class Piso
    {
        [Key]
        [Column("ID_PISO")]
        [Display(Name = "ID Piso")]
        public short IdPiso { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(20)]
        [Display(Name = "Descripcion")]
        public string? Descripcion { get; set; }

        [Column("Id_compania")]
        [Display(Name = "ID Compania")]
        public int? IdCompania { get; set; }

        [Column("id_edificio")]
        [Display(Name = "ID Edificio")]
        public int? IdEdificio { get; set; }

        [Column("rv")]
        [Timestamp]
        [Display(Name = "RV")]
        public byte[]? Rv { get; set; }

        [Column("ID_PISO_ANTERIOR")]
        [Display(Name = "ID Piso Anterior")]
        public int? IdPisoAnterior { get; set; }
    }
}
