using PokemonReview.BaseRepository;
using PokemonReview.Data;

namespace PokemonReview.Repository
{
    public class ReviewerRepo : MainRepo<Reviewer>, IReviewerRepo
    {
        private readonly AppDbContext context;

        public ReviewerRepo(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        
    }
}
