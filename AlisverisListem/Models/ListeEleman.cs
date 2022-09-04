using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace AlisverisListem.Models
{
    public partial class ListeEleman
    {
        public ListeEleman()
        {
            AktifMi = true;
        }
        public int ListeElemanId { get; set; }
        public int? ListeId { get; set; }
        public int? UrunId { get; set; }
        [StringLength(250,ErrorMessage ="Notunuz 100 karakteri geçemez!")]
        public string? UrunNot { get; set; }
        public bool? AktifMi { get; set; }

        public virtual Listeler? Liste { get; set; }
        public virtual Urunler? Urun { get; set; }
    }
}
