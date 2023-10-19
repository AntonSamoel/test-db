namespace PokemonReview.BaseRepository
{
    public interface IReviewRepo : IMainRepo<Review>
    {
        public ICollection<Review> GetReviewsOfPokemon(int pokemonId);
        public ICollection<Review> GetReviewsOfReviewer(int reviewerId);
    }
}
