namespace PokemonReview.BaseRepository
{
    public interface IOwnerRepo : IMainRepo<Owner>
    {
        ICollection<Owner> GetOwnersOfPokemon(int pokemonId);
        ICollection<Owner> GetOwnersOfCountry(int countryId);

    }
}
