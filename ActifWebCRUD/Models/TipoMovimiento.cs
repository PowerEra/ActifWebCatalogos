using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("tipo_movimiento")]
    public class TipoMovimiento
    {
        [Key]
        [Column("ID_TIPO_MOV")]
        [Display(Name = "ID Tipo Movimiento")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short IdTipoMov { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(250)]
        [Display(Name = "Descripcion")]
        public string? Descripcion { get; set; }

        [Column("FECHA_CAPTURA")]
        [Display(Name = "Fecha Captura")]
        public DateTime? FechaCaptura { get; set; }
    }
}
