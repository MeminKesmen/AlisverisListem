using AlisverisListem.Models;
using AlisverisListem.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AlisverisListem.Repositories
{
    public class ListeElemanRepository : GenericRepository<ListeEleman>
    {
        public List<ListeElemanViewModel> ElemanListe(int listeId)
        {

            List<ListeEleman> elemanlar = Sorgu().Include(x => x.Urun).Where(u => u.ListeId == listeId).ToList();

            return ElemanToViewModelList(elemanlar);
        }
        public ListeElemanViewModel ElemanGetir(int elemanId)
        {

           ListeEleman urunler = Sorgu().Include(x => x.Urun).Where(u => u.ListeElemanId == elemanId).FirstOrDefault();

            return ElemanToViewModel(urunler);
        }
         ListeElemanViewModel ElemanToViewModel(ListeEleman eleman)
        {
            ListeElemanViewModel elemanlarViews = new ListeElemanViewModel()
            {
                ListeElemanId = eleman.ListeElemanId,
                UrunAd = eleman.Urun.UrunAd,
                UrunResim = eleman.Urun.UrunResim,
                Not = eleman.UrunNot,
                AktifMi = (bool)eleman.AktifMi
            }; 
            return elemanlarViews;
        }


        List<ListeElemanViewModel> ElemanToViewModelList(List<ListeEleman> elemanlar)
        {
            List<ListeElemanViewModel> elemanlarViews = new List<ListeElemanViewModel>();
            elemanlar.ForEach(e => elemanlarViews.Add(new ListeElemanViewModel()
            {
                ListeElemanId = e.ListeElemanId,
                UrunAd = e.Urun.UrunAd,
                UrunResim = e.Urun.UrunResim,
                Not = e.UrunNot,
                AktifMi = (bool)e.AktifMi
            }));
            

            return elemanlarViews;
        }
    }
}
