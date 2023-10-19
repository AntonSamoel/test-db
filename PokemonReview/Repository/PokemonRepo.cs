using PokemonReview.BaseRepository;
using PokemonReview.Data;
using PokemonReview.Models;

namespace PokemonReview.Repository
{
    public class PokemonRepo : MainRepo<Pokemon>, IPokemonRepo
    {
        private readonly AppDbContext context;

        public PokemonRepo(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var owner = context.Owners.Find(ownerId);
            var category = context.Categories.Find(categoryId);

            var pokemonOwner = new PokemonOwner { Owner = owner, Pokemon = pokemon };
            var pokemonCategory = new PokemonCategory { Category = category, Pokemon = pokemon };

            context.PokemonCategories.Add(pokemonCategory);
            context.PokemonOwners.Add(pokemonOwner);
            context.Pokemons.Add(pokemon);

           return Save();
        }

        public Pokemon GetByName(string name)
        {
            return context.Pokemons.FirstOrDefault(p => p.Name == name);
        }

        public ICollection<Pokemon> GetPokemonsOfOwner(int ownerId)
        {
            return context.PokemonOwners.Where(pc=>pc.OwnerId == ownerId).Select(pc => pc.Pokemon).ToList();
        }
        public ICollection<Pokemon> GetPokemonsOfCategory(int categoryId)
        {
            return context.PokemonCategories.Where(pc=>pc.CategoryId == categoryId).Select(pc => pc.Pokemon).ToList();
        }

        public decimal GetRate(int id)
        {
            var rate = context.Reviews.Where(r => r.PokemonId == id);
            if(rate.Count() <= 0)
                return 0;
            return (decimal)(rate.Average(r => r.Rating));
        }

        public bool UpdatePokemon(Pokemon pokemon)
        { 
            context.Pokemons.Update(pokemon);
            return Save();
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            var pokemonCategories = context.PokemonCategories.Where(pc => pc.PokemonId == pokemon.Id).ToList();
            var pokemonsOwners = context.PokemonOwners.Where(pc => pc.PokemonId == pokemon.Id).ToList();
            context.PokemonCategories.RemoveRange(pokemonCategories);
            context.PokemonOwners.RemoveRange(pokemonsOwners);
            context.Pokemons.Remove(pokemon);
            return Save();
        }
    }
}
