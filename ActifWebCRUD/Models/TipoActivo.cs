using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("tipo_activo")]
    public class TipoActivo
    {
        [Key]
        [Column("ID_TIPO_ACTIVO")]
        [Display(Name = "ID Tipo Activo")]
        [Required(ErrorMessage = "El ID es obligatorio")]
        public short IdTipoActivo { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(250)]
        [Display(Name = "Descripcion")]
        public string? Descripcion { get; set; }

        [Column("rv")]
        [Display(Name = "RV")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Timestamp]
        public byte[]? Rv { get; set; }

        [Column("ID_COMPANIA")]
        [Display(Name = "Compania")]
        public short? IdCompania { get; set; }

        // Navigation property
        [ForeignKey("IdCompania")]
        public virtual Compania? Compania { get; set; }
    }
}
