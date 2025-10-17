using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("edificio")]
    public class Edificio
    {
        [Key]
        [Column("ID_EDIFICIO")]
        [Display(Name = "ID Edificio")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEdificio { get; set; }

        [Column("ID_COMPANIA")]
        [Display(Name = "Compañía")]
        [Required(ErrorMessage = "La compañía es obligatoria")]
        public short IdCompania { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(50)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Column("CALLE_NUMERO")]
        [StringLength(50)]
        [Display(Name = "Calle y Número")]
        public string? CalleNumero { get; set; }

        [Column("COLONIA")]
        [StringLength(50)]
        [Display(Name = "Colonia")]
        public string? Colonia { get; set; }

        [Column("DELEG_MPIO")]
        [StringLength(50)]
        [Display(Name = "Delegación/Municipio")]
        public string? DelegMpio { get; set; }

        [Column("CODIGO_POSTAL")]
        [StringLength(10)]
        [Display(Name = "Código Postal")]
        public string? CodigoPostal { get; set; }

        [Column("ID_ESTADO")]
        [Display(Name = "Estado")]
        public short? IdEstado { get; set; }

        [Column("TELEFONO")]
        [StringLength(20)]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Column("CTA1")]
        [StringLength(50)]
        [Display(Name = "Cuenta 1")]
        public string? Cta1 { get; set; }

        [Column("CTA2")]
        [StringLength(50)]
        [Display(Name = "Cuenta 2")]
        public string? Cta2 { get; set; }

        [Column("CTA3")]
        [StringLength(50)]
        [Display(Name = "Cuenta 3")]
        public string? Cta3 { get; set; }

        [Column("CTA4")]
        [StringLength(50)]
        [Display(Name = "Cuenta 4")]
        public string? Cta4 { get; set; }

        [Column("CTA5")]
        [StringLength(50)]
        [Display(Name = "Cuenta 5")]
        public string? Cta5 { get; set; }

        [Column("CTA6")]
        [StringLength(50)]
        [Display(Name = "Cuenta 6")]
        public string? Cta6 { get; set; }

        [Column("INACTIVO")]
        [Display(Name = "Inactivo")]
        public int? Inactivo { get; set; }

        [Column("CIUDAD")]
        [StringLength(50)]
        [Display(Name = "Ciudad")]
        public string? Ciudad { get; set; }

        [Column("Id_Edificio_Orig")]
        [Display(Name = "ID Edificio Original")]
        public int? IdEdificioOrig { get; set; }

        [Column("rv")]
        [Timestamp]
        public byte[]? Rv { get; set; }

        [Column("Id_Compania_Orig")]
        [Display(Name = "ID Compañía Original")]
        public short? IdCompaniaOrig { get; set; }

        // Navigation properties (manually loaded)
        [NotMapped]
        public virtual Compania? Compania { get; set; }

        [NotMapped]
        public virtual Estado? Estado { get; set; }
    }
}
