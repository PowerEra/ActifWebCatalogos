using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("TiposCalculo")]
    public class TipoCalculo
    {
        [Key]
        [Column("ID_TIPO_CALCULO")]
        [Display(Name = "ID Tipo Cálculo")]
        public int IdTipoCalculo { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(20)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }
    }
}
