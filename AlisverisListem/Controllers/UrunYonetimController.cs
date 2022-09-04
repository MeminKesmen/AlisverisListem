using AlisverisListem.Models;
using AlisverisListem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace AlisverisListem.Controllers
{
    public class UrunYonetimController : Controller
    {
        UrunRepository urunRepository;
        KategoriRepository kategoriRepository;
        public UrunYonetimController()
        {
            urunRepository = new UrunRepository();
            kategoriRepository = new KategoriRepository();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Urunler(string urunAd, int? kategoriId, int sayfa = 1)
        {
            if (urunAd != null)
            {
                ViewBag.urunAd = urunAd;
                return View("Urunler", urunRepository.UrunAra(urunAd).ToPagedList(sayfa, 16));
            }
            if (kategoriId != null)
            {
                ViewBag.kategoriId = kategoriId;
                return View("Urunler", urunRepository.KategoriUrunler((int)kategoriId).ToPagedList(sayfa, 16));
            }
            return View(urunRepository.UrunListe().ToPagedList(sayfa, 16));
        }
        [Authorize(Roles = "Admin")]
        public IActionResult KategoriUrunler(int id)
        {

            return View("Urunler", urunRepository.KategoriUrunler(id).ToPagedList());
        }
        [Authorize(Roles = "Admin")]
        public IActionResult UrunGuncelle(int id)
        {
            Urunler urun = urunRepository.Getir(id);
            if (urun == null) { return RedirectToAction("Urunler"); }
            ViewBag.KategoriListe = kategoriRepository.KategoriSelectlist((int)urun.KategoriId);

            return View(urun);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UrunGuncelle(UrunEkle urun)
        {
            Urunler u = urunRepository.Getir(urun.UrunId);
            if (!ModelState.IsValid)
            {
                ViewBag.KategoriListe = kategoriRepository.KategoriSelectlist((int)urun.KategoriId);
                return View(u);
            }
            u.UrunAd = urun.UrunAd;
            u.KategoriId = urun.KategoriId;
            if (urun.UrunResim != null)
            {
                if (u.UrunResim != null)
                {
                    urunRepository.ResimSil(u);
                }
                u.UrunResim = urunRepository.ResimYükle(urun);
            }
            urunRepository.Guncelle(u);
            return RedirectToAction("Urunler");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult UrunSil(int id)
        {
            Urunler urun = urunRepository.Getir(id);
            if (urun == null) { return RedirectToAction("Urunler"); }
            int sonuc = urunRepository.Sil(urun);
            if (sonuc < 1) { }
            else
            {
                if (urun.UrunResim != null)
                {
                    urunRepository.ResimSil(urun);
                }
            }
            return RedirectToAction("Urunler");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult UrunEkle()
        {
            ViewBag.KategoriListe = kategoriRepository.KategoriSelectlist();
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UrunEkle(UrunEkle urun)
        {
            if (!ModelState.IsValid || urun.UrunResim == null)
            {
                if (urun.UrunResim == null) { ModelState.AddModelError("UrunResim", "Resmi yükleyiniz!"); }

                ViewBag.KategoriListe = kategoriRepository.KategoriSelectlist(); return View();
            }
            Urunler u = new Urunler();
            if (urun.UrunResim != null)
            {
                u.UrunResim = urunRepository.ResimYükle(urun);
            }
            u.UrunAd = urun.UrunAd;
            u.KategoriId = urun.KategoriId;
            urunRepository.Ekle(u);
            return RedirectToAction("Urunler");
        }
    }
}
