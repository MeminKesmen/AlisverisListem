using AlisverisListem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlisverisListem.Repositories
{
    public class KategoriRepository:GenericRepository<Kategoriler>
    {

        
        public List<SelectListItem> KategoriSelectlist(int selected=1)
        {
            List<SelectListItem> kategoriler = (from k in Sorgu().ToList()
                                                select new SelectListItem
                                                {
                                                    Text = k.KategoriAd,
                                                    Value = k.KategoriId.ToString(),
                                                    Selected = (k.KategoriId == selected ? true : false)

                                                }).ToList();
            return kategoriler;
        }
    }
}
