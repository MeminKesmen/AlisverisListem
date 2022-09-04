using AlisverisListem.Models;
using AlisverisListem.Repositories;
using AlisverisListem.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AlisverisListem.Controllers
{
    public class AccountController : Controller
    {
        KullaniciRepository kullaniciRepository;
        public AccountController()
        {
            kullaniciRepository = new KullaniciRepository();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("login","Giriş bilgileriniz kontrol ediniz!");
                return View();
            }
            var kullanici = kullaniciRepository.Sorgu().FirstOrDefault(k => k.KullaniciMail == user.Mail && k.KullaniciSifre == user.Sifre);
            if (kullanici != null)
            {
                await HttpContext.SignOutAsync();
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, kullanici.KullaniciAd));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, kullanici.KullaniciId.ToString()));
                claims.Add(new Claim("uid", kullanici.KullaniciId.ToString()));
                if (kullanici.KullaniciMail == "admin@mail.com" && kullanici.KullaniciSifre == "Sifre1234")
                {

                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(principal);


                    return RedirectToAction("Urunler", "UrunYonetim");

                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, "User"));
                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(principal);

                    return RedirectToAction("Listelerim", "Home");
                }

            }

            ModelState.AddModelError("login", "Giriş bilgileriniz kontrol ediniz!");
            return View();
        }
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.SetString("listeId", "");
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login");
        }
        public IActionResult KayitOl()
        {
            return View();
        }
        [HttpPost]
        public IActionResult KayitOl(Kullanicilar kullanicilar, string? sifreTekrar)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (kullanicilar.KullaniciSifre != sifreTekrar)
            {
                ModelState.AddModelError("sifreTekrar", "Şifre tekrar bilginizi kontrol ediniz!");
                return View();
            }
            var kullanici = kullaniciRepository.Sorgu().FirstOrDefault(k => k.KullaniciMail == kullanicilar.KullaniciMail);
            if (kullanici != null)
            {
                ModelState.AddModelError("KullaniciMail", "Kullanıcı mevcut!");
                return View();
            }
            kullaniciRepository.Ekle(kullanicilar);
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
