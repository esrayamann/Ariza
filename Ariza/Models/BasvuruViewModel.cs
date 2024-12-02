using System.ComponentModel.DataAnnotations;

namespace Ariza.Models
{
    public class BasvuruViewModel
    {
        [Required(ErrorMessage = "Başvuru nedeni gereklidir.")]
        public string Ariza { get; set; }

        [Required(ErrorMessage = "Adres bilgisi gereklidir.")]
        public string Bolum { get; set; }
    }
}
