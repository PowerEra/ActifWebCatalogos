using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("INPC2")]
    public class INPC2
    {
        [Key]
        [Column("Id_INPC")]
        [Display(Name = "ID INPC")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdInpc { get; set; }

        [Column("Anio")]
        [Display(Name = "Año")]
        public int? Anio { get; set; }

        [Column("Mes")]
        [Display(Name = "Mes")]
        public int? Mes { get; set; }

        [Column("Id_Grupo_Simulacion")]
        [Display(Name = "Grupo Simulación")]
        public int? IdGrupoSimulacion { get; set; }

        [Column("Id_Pais")]
        [Display(Name = "País")]
        public int? IdPais { get; set; }

        [Column("Indice")]
        [Display(Name = "Índice")]
        public decimal? Indice { get; set; }

        // Navigation properties
        [ForeignKey("IdGrupoSimulacion")]
        public virtual GrupoSimulacion? GrupoSimulacion { get; set; }

        [ForeignKey("IdPais")]
        public virtual Pais? Pais { get; set; }
    }
}
