using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("grupoEdificio")]
    public class GrupoEdificio
    {
        [Key]
        [Column("IdGrupo")]
        [Display(Name = "ID Grupo")]
        public int IdGrupo { get; set; }

        [Column("Grupo")]
        [StringLength(20)]
        [Display(Name = "Grupo")]
        public string? Grupo { get; set; }
    }
}
