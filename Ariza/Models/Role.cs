using System.ComponentModel.DataAnnotations;

namespace Ariza.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Rol { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
