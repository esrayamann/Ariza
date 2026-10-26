using System.ComponentModel.DataAnnotations;

namespace Ariza.Models
{
    public class BasvuruViewModel
    {
        [Required(ErrorMessage = "Başvuru nedeni gereklidir.")]
        [StringLength(1000, ErrorMessage = "Arıza açıklaması en fazla 1000 karakter olabilir.")]
        public string Ariza { get; set; } = string.Empty;

        [Required(ErrorMessage = "Birim bilgisi gereklidir.")]
        [StringLength(120)]
        public string Birim { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bölüm bilgisi gereklidir.")]
        [StringLength(120)]
        public string Bolum { get; set; } = string.Empty;
    }
}
