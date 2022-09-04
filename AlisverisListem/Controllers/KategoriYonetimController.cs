using AlisverisListem.Models;
using AlisverisListem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace AlisverisListem.Controllers
{
    public class KategoriYonetimController : Controller
    {
        UrunRepository urunRepository;
        KategoriRepository kategoriRepository;
        public KategoriYonetimController()
        {
            urunRepository = new UrunRepository();
            kategoriRepository = new KategoriRepository();
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Kategoriler(int sayfa = 1)
        {

            return View(kategoriRepository.Sorgu().ToPagedList(sayfa, 10));
        }
        [Authorize(Roles = "Admin")]
        public IActionResult KategoriGuncelle(int id)
        {
            Kategoriler kategori = kategoriRepository.Getir(id);
            if (kategori == null) { return RedirectToAction("Kategoriler"); }


            return View(kategori);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult KategoriGuncelle(Kategoriler kategori)
        {
            if (!ModelState.IsValid)
            {
                return View(kategori);
            }
            kategoriRepository.Guncelle(kategori);



            return RedirectToAction("Kategoriler");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult KategoriSil(int id)
        {
            Kategoriler kategori = kategoriRepository.Getir(id);
            if (kategori == null) { return RedirectToAction("Kategoriler"); }
            urunRepository.Sorgu().Where(u => u.KategoriId == id).ToList().ForEach(u => urunRepository.ResimSil(u));
            kategoriRepository.Sil(kategori);


            return RedirectToAction("Kategoriler");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult KategoriEkle()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult KategoriEkle(Kategoriler kategori)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            kategoriRepository.Ekle(kategori);



            return RedirectToAction("Kategoriler");
        }


    }
}
