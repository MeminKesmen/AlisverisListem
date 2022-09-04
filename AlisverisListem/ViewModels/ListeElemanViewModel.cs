using System.ComponentModel.DataAnnotations;

namespace AlisverisListem.ViewModels
{
    public class ListeElemanViewModel
    {
        public int ListeElemanId { get; set; }
        public string UrunAd { get; set; }
        public string UrunResim { get; set; }
        [StringLength(250, ErrorMessage = "Notunuz 100 karakteri geçemez!")]
        public string? Not { get; set; }
        public bool AktifMi { get; set; }
    }
}
