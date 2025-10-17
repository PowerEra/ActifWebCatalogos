using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActifWebCRUD.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("IdUser")]
        public int IdUser { get; set; }

        [Column("UserName")]
        [StringLength(100)]
        public string? UserName { get; set; }

        [Column("Login")]
        [StringLength(50)]
        public string? Login { get; set; }

        [Column("Password")]
        [StringLength(100)]
        public string? Password { get; set; }

        [Column("Email")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Column("DefaultPermission")]
        public bool? DefaultPermission { get; set; }

        [Column("RecordDate")]
        public DateTime? RecordDate { get; set; }

        [Column("EditDate")]
        public DateTime? EditDate { get; set; }

        [Column("IdGrantorUser")]
        public int? IdGrantorUser { get; set; }

        [Column("Inactive")]
        public bool? Inactive { get; set; }
    }
}
