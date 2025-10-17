using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("estado_activo")]
    public class EstadoActivo
    {
        [Key]
        [Column("ID_ESTADO_ACTIVO")]
        [Display(Name = "ID Estado Activo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short IdEstadoActivo { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(20)]
        [Display(Name = "Descripcion")]
        public string? Descripcion { get; set; }

        [Column("rv")]
        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public byte[]? Rv { get; set; }
    }
}
