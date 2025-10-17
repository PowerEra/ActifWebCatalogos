using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("vEdificio")]
    public class VEdificio
    {
        [Key]
        [Column("ID_EDIFICIO")]
        [Display(Name = "ID Edificio")]
        public int IdEdificio { get; set; }

        [Column("ID_COMPANIA")]
        [Display(Name = "ID Compañía")]
        public short? IdCompania { get; set; }

        [Column("COMPANIA")]
        [Display(Name = "Compañía")]
        public string? Compania { get; set; }

        [Column("DESCRIPCION")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Column("CALLE_NUMERO")]
        [Display(Name = "Calle y Número")]
        public string? CalleNumero { get; set; }

        [Column("COLONIA")]
        [Display(Name = "Colonia")]
        public string? Colonia { get; set; }

        [Column("DELEG_MPIO")]
        [Display(Name = "Delegación/Municipio")]
        public string? DelegMpio { get; set; }

        [Column("CODIGO_POSTAL")]
        [Display(Name = "Código Postal")]
        public string? CodigoPostal { get; set; }

        [Column("ID_ESTADO")]
        [Display(Name = "ID Estado")]
        public short? IdEstado { get; set; }

        [Column("TELEFONO")]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Column("INACTIVO")]
        [Display(Name = "Inactivo")]
        public int? Inactivo { get; set; }

        [Column("Id_Edificio_Orig")]
        [Display(Name = "ID Edificio Original")]
        public int? IdEdificioOrig { get; set; }

        [Column("CTA2")]
        [Display(Name = "Cuenta 2")]
        public string? Cta2 { get; set; }

        [Column("CTA3")]
        [Display(Name = "Cuenta 3")]
        public string? Cta3 { get; set; }

        [Column("CTA4")]
        [Display(Name = "Cuenta 4")]
        public string? Cta4 { get; set; }

        [Column("CTA5")]
        [Display(Name = "Cuenta 5")]
        public string? Cta5 { get; set; }

        [Column("CTA6")]
        [Display(Name = "Cuenta 6")]
        public string? Cta6 { get; set; }
    }
}
