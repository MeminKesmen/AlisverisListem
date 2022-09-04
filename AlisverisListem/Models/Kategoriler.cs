using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace AlisverisListem.Models
{
    public partial class Kategoriler
    {
        public Kategoriler()
        {
            Urunlers = new HashSet<Urunler>();
        }

        public int KategoriId { get; set; }
        [Required(ErrorMessage ="Kategori adı boş geçilemez!")]
        [StringLength(50,ErrorMessage ="Kategori adı 30 karakteri geçemez!")]
        public string? KategoriAd { get; set; }

        public virtual ICollection<Urunler> Urunlers { get; set; }
    }
}
