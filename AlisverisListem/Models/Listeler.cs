using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace AlisverisListem.Models
{
    public partial class Listeler
    {
        public Listeler()
        {
            ListeElemen = new HashSet<ListeEleman>();
            AktifMi = true;
        }

        public int ListeId { get; set; }
        public int? KullaniciId { get; set; }
        [Required(ErrorMessage ="Liste adını giriniz!")]
        public string? ListeAd { get; set; }
        public bool? AktifMi { get; set; }

        public virtual Kullanicilar? Kullanici { get; set; }
        public virtual ICollection<ListeEleman> ListeElemen { get; set; }
    }
}
