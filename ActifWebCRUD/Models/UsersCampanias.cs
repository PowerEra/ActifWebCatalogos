using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("users_campanias")]
    public class UsersCampanias
    {
        [Key]
        [Column("ID_USER_COMPANIA")]
        [Display(Name = "ID User Compania")]
        public int IdUserCompania { get; set; }

        [Column("IDUSER")]
        [Display(Name = "Usuario")]
        public int? IdUser { get; set; }

        [Column("IDCOMPANIA")]
        [Display(Name = "Compania")]
        public short? IdCompania { get; set; }

        [Column("RV")]
        [Timestamp]
        [Display(Name = "RV")]
        public byte[]? Rv { get; set; }

        // Navigation properties
        [ForeignKey("IdUser")]
        public virtual User? User { get; set; }

        [ForeignKey("IdCompania")]
        public virtual Compania? Compania { get; set; }
    }
}
