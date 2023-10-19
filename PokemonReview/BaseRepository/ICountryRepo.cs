using PokemonReview.Models;

namespace PokemonReview.BaseRepository
{
    public interface ICountryRepo : IMainRepo<Country>
    {
        public Country GetCountryOfOwner(int ownerId);
    }
}
