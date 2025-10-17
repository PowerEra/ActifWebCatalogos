using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("significado_id")]
    public class SignificadoId
    {
        [Key]
        [Column("ID_SIGNIFICADO_ID")]
        [Display(Name = "ID Significado ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short IdSignificadoId { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(25)]
        [Display(Name = "Descripcion")]
        public string? Descripcion { get; set; }

        [Column("rv")]
        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public byte[]? Rv { get; set; }
    }
}
