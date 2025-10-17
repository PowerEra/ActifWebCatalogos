using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("clase_tipo_cambio")]
    public class ClaseTipoCambio
    {
        [Key]
        [Column("ID_CLASE_TIPCAM")]
        [Display(Name = "ID Clase Tipo Cambio")]
        public short IdClaseTipCam { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(25)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }
    }
}
