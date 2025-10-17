using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("vConcepto_contable")]
    public class VConceptoContable
    {
        [Key]
        [Column("ID_CONCEPTO")]
        [Display(Name = "ID Concepto")]
        public short IdConcepto { get; set; }

        [Column("ID_COMPANIA")]
        [Display(Name = "ID Compañía")]
        public short? IdCompania { get; set; }

        [Column("COMPANIA")]
        [Display(Name = "Compañía")]
        public string? Compania { get; set; }

        [Column("TIPO_ACTIVO")]
        [Display(Name = "Tipo Activo")]
        public string? TipoActivo { get; set; }

        [Column("SUBTIPO_ACTIVO")]
        [Display(Name = "Subtipo Activo")]
        public string? SubtipoActivo { get; set; }

        [Column("VARIABLE")]
        [Display(Name = "Variable")]
        public string? Variable { get; set; }

        [Column("DESCRIPCION")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Column("CTA101")]
        [Display(Name = "Cuenta 101")]
        public string? Cta101 { get; set; }

        [Column("CTA102")]
        [Display(Name = "Cuenta 102")]
        public string? Cta102 { get; set; }

        [Column("CTA103")]
        [Display(Name = "Cuenta 103")]
        public string? Cta103 { get; set; }

        [Column("CTA104")]
        [Display(Name = "Cuenta 104")]
        public string? Cta104 { get; set; }

        [Column("CTA105")]
        [Display(Name = "Cuenta 105")]
        public string? Cta105 { get; set; }

        [Column("CTA106")]
        [Display(Name = "Cuenta 106")]
        public string? Cta106 { get; set; }

        [Column("CAAB01")]
        [Display(Name = "CAAB 01")]
        public short? Caab01 { get; set; }

        [Column("DESG01")]
        [Display(Name = "DESG 01")]
        public short? Desg01 { get; set; }

        [Column("PROR01")]
        [Display(Name = "PROR 01")]
        public short? Pror01 { get; set; }

        [Column("CTA201")]
        [Display(Name = "Cuenta 201")]
        public string? Cta201 { get; set; }

        [Column("CTA202")]
        [Display(Name = "Cuenta 202")]
        public string? Cta202 { get; set; }

        [Column("CTA203")]
        [Display(Name = "Cuenta 203")]
        public string? Cta203 { get; set; }

        [Column("CTA204")]
        [Display(Name = "Cuenta 204")]
        public string? Cta204 { get; set; }

        [Column("CTA205")]
        [Display(Name = "Cuenta 205")]
        public string? Cta205 { get; set; }

        [Column("CTA206")]
        [Display(Name = "Cuenta 206")]
        public string? Cta206 { get; set; }

        [Column("CAAB02")]
        [Display(Name = "CAAB 02")]
        public short? Caab02 { get; set; }

        [Column("DESG02")]
        [Display(Name = "DESG 02")]
        public short? Desg02 { get; set; }

        [Column("PROR02")]
        [Display(Name = "PROR 02")]
        public short? Pror02 { get; set; }

        [Column("CTA301")]
        [Display(Name = "Cuenta 301")]
        public string? Cta301 { get; set; }

        [Column("CTA302")]
        [Display(Name = "Cuenta 302")]
        public string? Cta302 { get; set; }

        [Column("CTA303")]
        [Display(Name = "Cuenta 303")]
        public string? Cta303 { get; set; }

        [Column("CTA304")]
        [Display(Name = "Cuenta 304")]
        public string? Cta304 { get; set; }

        [Column("CTA305")]
        [Display(Name = "Cuenta 305")]
        public string? Cta305 { get; set; }

        [Column("CTA306")]
        [Display(Name = "Cuenta 306")]
        public string? Cta306 { get; set; }

        [Column("CAAB03")]
        [Display(Name = "CAAB 03")]
        public short? Caab03 { get; set; }

        [Column("DESG03")]
        [Display(Name = "DESG 03")]
        public short? Desg03 { get; set; }

        [Column("PROR03")]
        [Display(Name = "PROR 03")]
        public short? Pror03 { get; set; }

        [Column("EDIFICIO")]
        [Display(Name = "Edificio")]
        public string? Edificio { get; set; }
    }
}
