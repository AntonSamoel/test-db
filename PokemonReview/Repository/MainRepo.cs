using Microsoft.AspNetCore.Mvc;
using PokemonReview.BaseRepository;
using PokemonReview.Data;

namespace PokemonReview.Repository
{
    public class MainRepo<T> : IMainRepo<T> where T : class
    {
        private readonly AppDbContext context;

        public MainRepo(AppDbContext _context)
        {
            context = _context;
        }

        public bool Create(T entity)
        {
            context.Set<T>().Add(entity);
            return Save();
        }

        public bool Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            return Save();
        }

        public bool Exist(int id)
        {
            var item = context.Set<T>().Find(id);
            if(item==null) return false;
            return true;
        }

        public ICollection<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return context.Set<T>().Find(id);
        }

        public bool Save()
        {
            var saved = context.SaveChanges();
            return saved > 0;
        }

        public bool Update(T entity)
        {
            context.Set<T>().Update(entity);
            return Save();
        }
    }
}
