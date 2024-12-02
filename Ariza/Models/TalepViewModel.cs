using System.ComponentModel.DataAnnotations;

namespace Ariza.Models
{
    public class TalepViewModel
    {
        [Required(ErrorMessage = "Arıza açıklaması gereklidir.")]
        public string Ariza { get; set; }

        [Required(ErrorMessage = "Bölüm bilgisi gereklidir.")]
        public string Bolum { get; set; }

        [Required(ErrorMessage = "Birimi belirtmelisiniz.")]
        public string Birim { get; set; }

        [Required(ErrorMessage = "Telefon numarası gereklidir.")]
        public int TelNo { get; set; }

        [Required(ErrorMessage = "Email adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; }
    }
}
