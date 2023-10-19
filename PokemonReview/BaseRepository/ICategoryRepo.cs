using PokemonReview.Models;

namespace PokemonReview.BaseRepository
{
    public interface ICategoryRepo : IMainRepo<Category>
    {
        public ICollection<Pokemon> GetPokemonsByCategotyId(int categotyId);
    }
}
