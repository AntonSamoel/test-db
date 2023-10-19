using Microsoft.AspNetCore.Mvc;

namespace PokemonReview.BaseRepository
{
    public interface IMainRepo<T> where T : class
    {
        public T GetById(int id);
        public ICollection<T> GetAll();

        public bool Exist(int id);
        public bool Create(T entity);
        public bool Update(T entity);
        public bool Delete(T entity);
        public bool Save();
       
    }
}
