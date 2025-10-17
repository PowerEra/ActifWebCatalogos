using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("actif_config_placa")]
    public class ActifConfigPlaca
    {
        [Key]
        [Column("ID_CONFIG_PLACA")]
        [Display(Name = "ID Config Placa")]
        public int IdConfigPlaca { get; set; }

        [Column("ID_COMPANIA")]
        [Display(Name = "Compania")]
        public int? IdCompania { get; set; }

        [Column("PREFIJO")]
        [StringLength(250)]
        [Display(Name = "Prefijo")]
        public string? Prefijo { get; set; }

        [Column("MIN_DIGITOS")]
        [Display(Name = "Min Digitos")]
        public int? MinDigitos { get; set; }

        [Column("MAX_DIGITOS")]
        [Display(Name = "Max Digitos")]
        public int? MaxDigitos { get; set; }

        [Column("RV")]
        [Timestamp]
        public byte[]? Rv { get; set; }

        // Navigation property (manually loaded due to type mismatch)
        [NotMapped]
        public virtual Compania? Compania { get; set; }
    }
}
