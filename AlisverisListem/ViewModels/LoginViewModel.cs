using System.ComponentModel.DataAnnotations;
namespace AlisverisListem.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Mail { get; set; }
        [Required]
        public string Sifre { get; set; }
    }
}
