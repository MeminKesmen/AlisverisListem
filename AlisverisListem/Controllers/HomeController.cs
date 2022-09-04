using AlisverisListem.Models;
using AlisverisListem.Repositories;
using AlisverisListem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace AlisverisListem.Controllers
{
    public class HomeController : Controller
    {
        ListeRepository listeRepository;
        UrunRepository urunRepository;
        ListeElemanRepository listeElemanRepository;

        public HomeController()
        {


            listeRepository = new ListeRepository();
            urunRepository = new UrunRepository();
            listeElemanRepository = new ListeElemanRepository();
        }


        [Authorize]
        public IActionResult Listelerim()
        {
            int uid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid").Value);
            return View(listeRepository.Sorgu().Where(l => l.KullaniciId == uid).ToList());
        }
        [Authorize]
        public IActionResult ListeOlustur(Listeler liste)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Listelerim");
            }
            int uid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid").Value);
            liste.KullaniciId = uid;

            listeRepository.Ekle(liste);
            return RedirectToAction("Listelerim");
        }
        [Authorize]
        public IActionResult ListeSil(int id)
        {
            int uid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid").Value);
            var liste = listeRepository.Sorgu().Where(l => l.KullaniciId == uid && l.ListeId == id).FirstOrDefault();
            if (liste == null) { RedirectToAction("Listelerim"); }
            listeRepository.Sil(liste);
            return RedirectToAction("Listelerim");
        }
        [Authorize]
        public IActionResult ListeSec(int id)
        {
            int uid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid").Value);
            var liste = listeRepository.Sorgu().Where(l => l.KullaniciId == uid && l.ListeId == id).FirstOrDefault();
            if (liste == null || liste.AktifMi == false) { return RedirectToAction("Listelerim"); }
            HttpContext.Session.SetString("listeId", liste.ListeId.ToString());
            //seçili liste session id ye ekle
            return RedirectToAction("Urunler");
        }
        [Authorize]
        public IActionResult Urunler(string? urunAd, int? kategoriId, int sayfa = 1)
        {
            string listeId = HttpContext.Session.GetString("listeId");

            if (listeId == "" || listeId == null)
            {
                //veya seesion boş ise
                return RedirectToAction("Listelerim");
            }
            if (!(bool)listeRepository.Getir(int.Parse(listeId)).AktifMi)
            {
                //sessiondaki list aktiflik durumu false ise listeye geri dön uyarı verilebilr
                return RedirectToAction("Listelerim");
            }
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
        [Authorize]
        public IActionResult ListeyeEkle(int id)
        {
            //liste id session boş ise listelerime git
            string listeId = HttpContext.Session.GetString("listeId");

            if (listeId == "" || listeId == null)
            {

                return RedirectToAction("Listelerim");
            }
            var urun = urunRepository.Getir(id);
            if (urun == null) { return RedirectToAction("Urunler"); }

            return View(urun);
        }
        [Authorize]
        [HttpPost]
        public IActionResult ListeyeEkle(ListeEleman listeEleman)
        {
            if (!ModelState.IsValid) { return View(urunRepository.Getir((int)listeEleman.UrunId)); }
            listeEleman.ListeId = int.Parse(HttpContext.Session.GetString("listeId"));


            listeElemanRepository.Ekle(listeEleman);
            return RedirectToAction("Urunler");
        }
        [Authorize]
        public IActionResult ListeDetay(int id)
        {
            int uid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid").Value);
            Listeler liste = listeRepository.Sorgu().Where(l => l.KullaniciId == uid && l.ListeId == id).FirstOrDefault();
            if (liste == null) { return RedirectToAction("Listelerim"); }
            ListeDetayViewModel listeDetay = new ListeDetayViewModel();
            listeDetay.ListeId = liste.ListeId;
            listeDetay.ListeAd = liste.ListeAd;
            listeDetay.AktifMi = (bool)liste.AktifMi;

            listeDetay.ListeElemanlar = listeElemanRepository.ElemanListe(id);

            return View(listeDetay);
        }
        [Authorize]
        public IActionResult ListeElemanKaldir(int id)
        {
            int uid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid").Value);
            var eleman = listeElemanRepository.Sorgu().Include(x => x.Liste).Where(e => e.Liste.KullaniciId == uid&&e.ListeElemanId==id).FirstOrDefault();
            if (eleman == null) { return RedirectToAction("Listelerim"); }
            var liste = listeRepository.Getir((int)eleman.ListeId);
            if (liste.AktifMi == false) { return RedirectToAction("ListeDetay", routeValues: new { id = eleman.ListeId }); }
            listeElemanRepository.Sil(eleman);
            return RedirectToAction("ListeDetay", routeValues: new { id = eleman.ListeId });
        }
        [Authorize]
        public IActionResult ListeElemanGuncelle(int id)
        {
            int uid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid").Value);
            var eleman = listeElemanRepository.Sorgu().Include(x => x.Liste).Where(e => e.Liste.KullaniciId == uid && e.ListeElemanId == id).FirstOrDefault();
            if (eleman == null) { return RedirectToAction("Listelerim"); }
            if ((bool)eleman.AktifMi)
            {
                eleman.AktifMi = false;
            }
            else
            {
                eleman.AktifMi = true;
            }
            listeElemanRepository.Guncelle(eleman);
            return RedirectToAction("ListeDetay", routeValues: new { id = eleman.ListeId });
        }
        [Authorize]
        public IActionResult ListeGuncelle(int id)
        {
            int uid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid").Value);
            var liste = listeRepository.Sorgu().Where(l => l.KullaniciId == uid && l.ListeId == id).FirstOrDefault();
            if (liste == null) { RedirectToAction("Listelerim"); }
            if ((bool)liste.AktifMi)
            {
                liste.AktifMi = false;/*liste id session boşalt*/
                HttpContext.Session.SetString("listeId", "");
            }
            else
            {
                liste.AktifMi = true;
                listeElemanRepository.Sorgu().Where(x => x.ListeId == liste.ListeId && x.AktifMi == false).ToList().ForEach(x => listeElemanRepository.Sil(x));
            }
            listeRepository.Guncelle(liste);
            return RedirectToAction("ListeDetay", new { id = liste.ListeId });
        }
        [Authorize]
        public IActionResult ListeElemanDetay(int id)
        {
            int uid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid").Value);
            //bu eleman nın listesi bu kullanıcıya aitse işlem yap
            var listeEleman = listeElemanRepository.Sorgu().Include(x => x.Liste).Where(e => e.Liste.KullaniciId == uid && e.ListeElemanId == id).FirstOrDefault();
            if (listeEleman == null) { return RedirectToAction("Urunler"); }
            var liste = listeRepository.Getir((int)listeEleman.ListeId);
            if (liste.AktifMi == false) { return RedirectToAction("ListeDetay", routeValues: new { id = listeEleman.ListeId }); }
            return View(listeElemanRepository.ElemanGetir(id));
        }
        [Authorize]
        [HttpPost]
        public IActionResult ListeElemanDetay(int listeElemanId, string? not)
        {
            if (!ModelState.IsValid) { return View(listeElemanRepository.ElemanGetir(listeElemanId)); }
            var eleman = listeElemanRepository.Getir(listeElemanId);
            eleman.UrunNot = not;
            listeElemanRepository.Guncelle(eleman);
            return RedirectToAction("ListeDetay", routeValues: new { id = eleman.ListeId });
        }
    }
}
