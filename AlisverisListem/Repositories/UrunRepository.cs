using AlisverisListem.Models;
using AlisverisListem.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AlisverisListem.Repositories
{
    public class UrunRepository : GenericRepository<Urunler>
    {

        public List<UrunViewModel> UrunListe()
        {

            List<Urunler> urunler = Sorgu().Include(x=>x.Kategori).ToList();

            return UrunToViewModel(urunler);
        }
        public List<UrunViewModel> KategoriUrunler(int kategoriId)
        {
            
            List<Urunler> urunler = Sorgu().Include(x => x.Kategori).Where(u => u.KategoriId == kategoriId).ToList();

            return UrunToViewModel(urunler);
        }
        public List<UrunViewModel> UrunAra(string urunAd)
        {

            List<Urunler> urunler = Sorgu().Include(x => x.Kategori).Where(u => u.UrunAd.Contains(urunAd)).ToList();

            return UrunToViewModel(urunler);
        }
        List<UrunViewModel> UrunToViewModel(List<Urunler> urunler)
        {
            List<UrunViewModel> urunViews = new List<UrunViewModel>();
            urunler.ForEach(u=> urunViews.Add(new UrunViewModel() {
                UrunId = u.UrunId,
                UrunAd = u.UrunAd,
                UrunResim = u.UrunResim,
                UrunKategoriAd = u.Kategori.KategoriAd
            }) );
            

            return urunViews;
        }
        public string ResimYükle(UrunEkle urun)
        {
            string resimYolu = null;
            var extension = Path.GetExtension(urun.UrunResim.FileName);
            var yeniResim = Guid.NewGuid() + extension;
            var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Resimler/", yeniResim);
            using (var stream = new FileStream(location, FileMode.Create))
            {
                urun.UrunResim.CopyTo(stream);
                resimYolu = yeniResim;
            }

            return resimYolu;
        }
        public void ResimSil(Urunler urun)
        {
            var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Resimler/", urun.UrunResim);
            if (System.IO.File.Exists(location))
            { System.IO.File.Delete(location); }
        }



    }
}
