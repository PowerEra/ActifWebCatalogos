using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("grupoEdificioDetalle")]
    public class GrupoEdificioDetalle
    {
        [Key]
        [Column("IdGE")]
        [Display(Name = "ID")]
        public int IdGE { get; set; }

        [Column("IdGrupo")]
        [Display(Name = "Grupo")]
        public int? IdGrupo { get; set; }

        [Column("Id_Edificio")]
        [Display(Name = "Edificio")]
        public int? IdEdificio { get; set; }

        // Navigation properties
        [ForeignKey("IdGrupo")]
        public virtual GrupoEdificio? GrupoEdificio { get; set; }

        [ForeignKey("IdEdificio")]
        public virtual Edificio? Edificio { get; set; }
    }
}
