using System.Collections.Generic;
using Ariza.Models;

public class ChangeRoleViewModel
{
    public int UserId { get; set; }
    public string Ad { get; set; }
    public string Soyad { get; set; }
    public string Email { get; set; }
    public int CurrentRoleId { get; set; }
    public int SelectedRoleId { get; set; }
    public List<Role> AvailableRoles { get; set; }
}
