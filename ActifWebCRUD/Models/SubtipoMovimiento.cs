using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("subtipo_movimiento")]
    public class SubtipoMovimiento
    {
        [Key]
        [Column("ID_SUBTIPO_MOV")]
        [Display(Name = "ID Subtipo Movimiento")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short IdSubtipoMov { get; set; }

        [Column("ID_TIPO_MOV")]
        [Display(Name = "Tipo Movimiento")]
        public short? IdTipoMov { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(25)]
        [Display(Name = "Descripcion")]
        public string? Descripcion { get; set; }

        [Column("AFECTACION_INTERFAZ")]
        [StringLength(1)]
        [Display(Name = "Afectacion Interfaz")]
        public string? AfectacionInterfaz { get; set; }

        [Column("FLG_VISUALIZAR")]
        [StringLength(1)]
        [Display(Name = "Flag Visualizar")]
        public string? FlgVisualizar { get; set; }

        [Column("FLG_GRABAHIST")]
        [StringLength(1)]
        [Display(Name = "Flag Graba Historial")]
        public string? FlgGrabahist { get; set; }

        // Navigation property
        [ForeignKey("IdTipoMov")]
        public virtual TipoMovimiento? TipoMovimiento { get; set; }
    }
}
