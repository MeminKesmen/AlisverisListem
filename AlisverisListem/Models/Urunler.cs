using System;
using System.Collections.Generic;

namespace AlisverisListem.Models
{
    public partial class Urunler
    {
        public Urunler()
        {
            ListeElemen = new HashSet<ListeEleman>();
        }

        public int UrunId { get; set; }
        public string? UrunAd { get; set; }
        public string? UrunResim { get; set; }
        public int? KategoriId { get; set; }

        public virtual Kategoriler? Kategori { get; set; }
        public virtual ICollection<ListeEleman> ListeElemen { get; set; }
    }
}
