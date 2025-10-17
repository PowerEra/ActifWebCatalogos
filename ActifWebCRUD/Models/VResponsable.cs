using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("vResponsable")]
    public class VResponsable
    {
        [Key]
        [Column("ID_RESPONSABLE")]
        [Display(Name = "ID Responsable")]
        public int IdResponsable { get; set; }

        [Column("ID_COMPANIA")]
        [Display(Name = "ID Compania")]
        public short? IdCompania { get; set; }

        [Column("COMPANIA")]
        [Display(Name = "Compania")]
        public string? Compania { get; set; }

        [Column("CENTRO_COSTO")]
        [Display(Name = "Centro de Costo")]
        public string? CentroCosto { get; set; }

        [Column("NOMBRE")]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [Column("PUESTO_RESPONSABLE")]
        [Display(Name = "Puesto")]
        public string? PuestoResponsable { get; set; }

        [Column("NumeroEmpleado")]
        [Display(Name = "Numero de Empleado")]
        public int? NumeroEmpleado { get; set; }
    }
}
