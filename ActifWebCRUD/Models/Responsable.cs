using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("responsable")]
    public class Responsable
    {
        [Key]
        [Column("ID_RESPONSABLE")]
        [Display(Name = "ID Responsable")]
        public int IdResponsable { get; set; }

        [Column("ID_CENTRO_COSTO")]
        [Display(Name = "Centro de Costo")]
        public int? IdCentroCosto { get; set; }

        [Column("ID_COMPANIA")]
        [Display(Name = "Compania")]
        public short? IdCompania { get; set; }

        [Column("NOMBRE")]
        [StringLength(250)]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [Column("PUESTO_RESPONSABLE")]
        [StringLength(250)]
        [Display(Name = "Puesto")]
        public string? PuestoResponsable { get; set; }

        [Column("NumeroEmpleado")]
        [Display(Name = "Numero de Empleado")]
        public int? NumeroEmpleado { get; set; }

        [Column("rv")]
        [Timestamp]
        public byte[]? Rv { get; set; }
    }
}
