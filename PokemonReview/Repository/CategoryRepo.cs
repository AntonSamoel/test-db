using Microsoft.EntityFrameworkCore;
using PokemonReview.BaseRepository;
using PokemonReview.Data;
using PokemonReview.Models;

namespace PokemonReview.Repository
{
    public class CategoryRepo : MainRepo<Category>, ICategoryRepo
    {
        private readonly AppDbContext context;

        public CategoryRepo(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public ICollection<Pokemon> GetPokemonsByCategotyId(int categoryId)
        {
            return context.PokemonCategories.Where(pc => pc.CategoryId == categoryId).Select(pc => pc.Pokemon).ToList();
        }
       
    }
}
