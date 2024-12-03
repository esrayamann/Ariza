using System.ComponentModel.DataAnnotations;

namespace Ariza.Models
{
    public class Talep
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }//
        public virtual User User { get; set; }
        [Required(ErrorMessage = "Sicil Numarası gereklidir.")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Sicil Numarası 5 haneli olmalıdır.")]
        public string SicilNo { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string TelNo { get; set; }
        public string Email { get; set; }
        public string Ilgilenen { get; set; }
        public string Ariza { get; set; }
        public string Durum { get; set; }
        public string Birim { get; set; }
        public string Bolum { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
