using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Ariza.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Sifre { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Talep> Taleps { get; set; }

    }
}
