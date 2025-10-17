using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("subtipo_activo")]
    public class SubtipoActivo
    {
        [Key]
        [Column("ID_SUBTIPO_ACTIVO")]
        [Display(Name = "ID Subtipo Activo")]
        [Required(ErrorMessage = "El ID es obligatorio")]
        public int IdSubtipoActivo { get; set; }

        [Column("ID_TIPO_ACTIVO")]
        [Display(Name = "Tipo Activo")]
        [Required(ErrorMessage = "El tipo de activo es obligatorio")]
        public short IdTipoActivo { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(40)]
        [Display(Name = "Descripcion")]
        public string? Descripcion { get; set; }

        [Column("ACTUALIZAR")]
        [StringLength(1)]
        [Display(Name = "Actualizar")]
        public string? Actualizar { get; set; }

        [Column("Codigo")]
        [StringLength(6)]
        [Display(Name = "Codigo")]
        public string? Codigo { get; set; }

        [Column("rv")]
        [Display(Name = "RV")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Timestamp]
        public byte[]? Rv { get; set; }

        // Navigation property
        [ForeignKey("IdTipoActivo")]
        public virtual TipoActivo? TipoActivo { get; set; }
    }
}
