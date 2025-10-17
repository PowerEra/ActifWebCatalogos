using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("compania")]
    public class Compania
    {
        [Key]
        [Column("ID_COMPANIA")]
        [Display(Name = "ID Compania")]
        public short IdCompania { get; set; }

        [Column("NOMBRE")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(255)]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [Column("RFC")]
        [StringLength(50)]
        [Display(Name = "RFC")]
        public string? Rfc { get; set; }

        [Column("FECHA_INICIO_EJERC")]
        [Display(Name = "Fecha Inicio Ejercicio")]
        [DataType(DataType.Date)]
        public DateTime? FechaInicioEjerc { get; set; }

        [Column("ID_POLIZA_SIG")]
        [Display(Name = "ID Poliza Siguiente")]
        public int? IdPolizaSig { get; set; }

        [Column("VALOR_CERO")]
        [Display(Name = "Valor Cero")]
        public short? ValorCero { get; set; }

        [Column("CALLE_NUMERO")]
        [StringLength(255)]
        [Display(Name = "Calle y Numero")]
        public string? CalleNumero { get; set; }

        [Column("COLONIA")]
        [StringLength(100)]
        [Display(Name = "Colonia")]
        public string? Colonia { get; set; }

        [Column("DELEG_MPIO")]
        [StringLength(100)]
        [Display(Name = "Delegacion/Municipio")]
        public string? DelegMpio { get; set; }

        [Column("CODIGO_POSTAL")]
        [StringLength(10)]
        [Display(Name = "Codigo Postal")]
        public string? CodigoPostal { get; set; }

        [Column("ID_ESTADO")]
        [Display(Name = "ID Estado")]
        public short? IdEstado { get; set; }

        [Column("ID_SIG_RESPONSIVA")]
        [Display(Name = "ID Siguiente Responsiva")]
        public int? IdSigResponsiva { get; set; }

        [Column("TELEFONO")]
        [StringLength(50)]
        [Display(Name = "Telefono")]
        public string? Telefono { get; set; }

        [Column("CUENTA")]
        [StringLength(100)]
        [Display(Name = "Cuenta")]
        public string? Cuenta { get; set; }

        [Column("ID_CONTABLE")]
        [StringLength(100)]
        [Display(Name = "ID Contable")]
        public string? IdContable { get; set; }

        [Column("DIVISION")]
        [StringLength(100)]
        [Display(Name = "Division")]
        public string? Division { get; set; }

        [Column("Default_Id_Moneda")]
        [Display(Name = "ID Moneda Default")]
        public int? DefaultIdMoneda { get; set; }

        [Column("Default_Id_Pais")]
        [Display(Name = "ID Pais Default")]
        public int? DefaultIdPais { get; set; }

        [Column("Default_TipoCambio")]
        [Display(Name = "Tipo Cambio Default")]
        public decimal? DefaultTipoCambio { get; set; }

        [Column("ID_TIPO_DEP_PRINCIPAL")]
        [Display(Name = "ID Tipo Dep Principal")]
        public int? IdTipoDepPrincipal { get; set; }

        [Column("rv")]
        [Display(Name = "RV")]
        public byte[]? Rv { get; set; }

        [Column("RequerirDocumento")]
        [Display(Name = "Requerir Documento")]
        public int? RequerirDocumento { get; set; }

        [Column("RequerirAprobacion")]
        [Display(Name = "Requerir Aprobacion")]
        public int? RequerirAprobacion { get; set; }

        [Column("PrefijoRFID")]
        [StringLength(50)]
        [Display(Name = "Prefijo RFID")]
        public string? PrefijoRfid { get; set; }
    }
}
