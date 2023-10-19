using PokemonReview.BaseRepository;
using PokemonReview.Data;

namespace PokemonReview.Repository
{
    public class CountryRepo : MainRepo<Country>, ICountryRepo
    {
        private readonly AppDbContext context;

        public CountryRepo(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public Country GetCountryOfOwner(int ownerId)
        {
           return context.Owners.Where(o=>o.Id==ownerId).Select(o=>o.Country).FirstOrDefault();
        }

       

    }
}
