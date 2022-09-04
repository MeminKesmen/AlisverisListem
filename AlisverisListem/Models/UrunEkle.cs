using System.ComponentModel.DataAnnotations;
namespace AlisverisListem.Models
{
    public class UrunEkle
    {
        public int UrunId { get; set; }
        [Required(ErrorMessage = "Ürün adı boş geçilemez!")]
        [StringLength(maximumLength:30,ErrorMessage ="Ürün ad uzunluğu 30 karakteri geçemez!")]
        public string? UrunAd { get; set; }
        
        public IFormFile? UrunResim { get; set; }
        public int? KategoriId { get; set; }


    }
}
