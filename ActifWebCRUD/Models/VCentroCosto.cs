using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("vCentro_Costo")]
    public class VCentroCosto
    {
        [Key]
        [Column("ID_CENTRO_COSTO")]
        [Display(Name = "ID Centro Costo")]
        public int IdCentroCosto { get; set; }

        [Column("ID_COMPANIA")]
        [Display(Name = "ID Compañía")]
        public short? IdCompania { get; set; }

        [Column("COMPANIA")]
        [Display(Name = "Compañía")]
        public string? Compania { get; set; }

        [Column("Codigo")]
        [Display(Name = "Código")]
        public string? Codigo { get; set; }

        [Column("DESCRIPCION")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Column("RESPONSABLE")]
        [Display(Name = "Responsable")]
        public string? Responsable { get; set; }

        [Column("STATUS")]
        [Display(Name = "Status")]
        public short? Status { get; set; }
    }
}
