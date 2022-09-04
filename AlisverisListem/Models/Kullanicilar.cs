using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace AlisverisListem.Models
{
    public partial class Kullanicilar
    {
        public Kullanicilar()
        {
            Listelers = new HashSet<Listeler>();
        }

        public int KullaniciId { get; set; }
        [Required(ErrorMessage = "Ad boş geçilemez!")]
        [StringLength(20, ErrorMessage = "Kullanıcı adı en fazla 20 karakter olmalıdır!")]
        public string? KullaniciAd { get; set; }
        [Required(ErrorMessage = "Soyadı boş geçilemez!")]
        [StringLength(20, ErrorMessage = "Kullanıcı soyadı en fazla 20 karakter olmalıdır!")]
        public string? KullaniciSoyad { get; set; }
        [Required(ErrorMessage = "Mail boş geçilemez!")]
        public string? KullaniciMail { get; set; }
        [Required(ErrorMessage = "Sifre boş geçilemez!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$",
            ErrorMessage = "Şifreniz en az 8 karakter olmalı ve büyük harf, küçük harf ve rakam içermelidir!")]
        public string? KullaniciSifre { get; set; }

        public virtual ICollection<Listeler> Listelers { get; set; }
    }
}
