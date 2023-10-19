using PokemonReview.Models;

namespace PokemonReview.BaseRepository
{
    public interface IPokemonRepo 
    {
        public Pokemon GetById(int id);
        public ICollection<Pokemon> GetAll();

        public bool Exist(int id);
        public bool Create(Pokemon entity);
        public bool Save();

        public Pokemon GetByName(string name);
        public decimal GetRate(int id);
        public ICollection<Pokemon> GetPokemonsOfOwner(int ownerId);
        public ICollection<Pokemon> GetPokemonsOfCategory(int categoryId);
        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        public bool UpdatePokemon(Pokemon pokemon);
        public bool DeletePokemon(Pokemon pokemon);
    }
}
