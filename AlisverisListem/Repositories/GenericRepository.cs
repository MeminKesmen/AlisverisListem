using AlisverisListem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AlisverisListem.Repositories
{
    public class GenericRepository<T> where T : class, new()
    {
        DBAlisVerisListContext context;
        DbSet<T> dbSet;
        public GenericRepository()
        {
            context = new DBAlisVerisListContext();
            dbSet = context.Set<T>();
        }

        public IQueryable<T> Sorgu()
        {
            return dbSet.AsNoTracking();

        }

        public int Ekle(T p)
        {
            dbSet.Add(p);
            return context.SaveChanges();
        }
        public int Sil(T p)
        {
            dbSet.Remove(p);
            return context.SaveChanges();
        }
        public int Guncelle(T p)
        {
            dbSet.Update(p);
            return context.SaveChanges();
        }
        public T Getir(int id)
        {
            return dbSet.Find(id);
        }

    }
}
