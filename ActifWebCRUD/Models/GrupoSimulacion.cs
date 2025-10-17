using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("grupo_simulacion")]
    public class GrupoSimulacion
    {
        [Key]
        [Column("ID_GRUPO_SIMULACION")]
        [Display(Name = "ID Grupo Simulacion")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdGrupoSimulacion { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(20)]
        [Display(Name = "Descripcion")]
        public string? Descripcion { get; set; }

        [Column("NOTA")]
        [Display(Name = "Nota")]
        public string? Nota { get; set; }

        [Column("rv")]
        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public byte[]? Rv { get; set; }
    }
}
