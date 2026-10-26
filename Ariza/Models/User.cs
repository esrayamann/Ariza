using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Ariza.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        // Existing column name is preserved for migration safety; it now stores a password hash.
        public string Sifre { get; set; } = string.Empty;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<Talep> Taleps { get; set; } = new List<Talep>();

    }
}
