using PokemonReview.BaseRepository;
using PokemonReview.Data;

namespace PokemonReview.Repository
{
    public class OwnerRepo : MainRepo<Owner>, IOwnerRepo
    {
        private readonly AppDbContext context;

        public OwnerRepo(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public ICollection<Owner> GetOwnersOfCountry(int countryId)
        {
            return context.Owners.Where(o=>o.CountryId == countryId).ToList();
        }

        public ICollection<Owner> GetOwnersOfPokemon(int pokemonId)
        {
            return context.PokemonOwners.Where(pc => pc.PokemonId == pokemonId).Select(pc => pc.Owner).ToList();
        }
        
    }
}
