using System.ComponentModel.DataAnnotations;

namespace Ariza.Models
{
    public class Talep
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }//
        public virtual User User { get; set; } = null!;
        public int SicilNo { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public int TelNo { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Ilgilenen { get; set; } = string.Empty;
        public string Ariza { get; set; } = string.Empty;
        public string Durum { get; set; } = string.Empty;
        public string Birim { get; set; } = string.Empty;
        public string Bolum { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
