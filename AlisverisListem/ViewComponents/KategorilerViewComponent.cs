using AlisverisListem.Models;
using AlisverisListem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AlisverisListem.ViewComponents
{
    public class KategorilerViewComponent:ViewComponent
    {
        KategoriRepository kategoriRepository;
        IMemoryCache cache;
        public KategorilerViewComponent(IMemoryCache memoryCache)
        {
            cache = memoryCache;
            kategoriRepository = new KategoriRepository();
        }
        public IViewComponentResult Invoke()
        {
            List<Kategoriler> kategoriler;
            if (!cache.TryGetValue("kategoriler",out kategoriler))
            {
                kategoriler = kategoriRepository.Sorgu().ToList();
                MemoryCacheEntryOptions memoryCacheEntryOptions = new MemoryCacheEntryOptions();
                memoryCacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(5);
                cache.Set("kategoriler",kategoriler);
               
            }
            
            return View(kategoriler);
        }
    }
}
