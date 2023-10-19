using PokemonReview.BaseRepository;
using PokemonReview.Data;

namespace PokemonReview.Repository
{
    public class ReviewRepo : MainRepo<Review>, IReviewRepo
    {
        private readonly AppDbContext context;

        public ReviewRepo(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

  

        public bool CreateReview(int reviewerId, int pokeId, Review review)
        {
            review.PokemonId = pokeId;
            review.ReviewerId = reviewerId;
            context.Reviews.Add(review);
            return Save();
        }

        public ICollection<Review> GetReviewsOfPokemon(int pokemonId)
        {
            return context.Reviews.Where(r=>r.PokemonId == pokemonId).ToList();
        }

        public ICollection<Review> GetReviewsOfReviewer(int reviewerId)
        {
            return context.Reviews.Where(r=>r.ReviewerId == reviewerId).ToList();
        }
    }
}
