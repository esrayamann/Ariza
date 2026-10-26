using System.ComponentModel.DataAnnotations;

namespace Ariza.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Rol { get; set; } = string.Empty;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
