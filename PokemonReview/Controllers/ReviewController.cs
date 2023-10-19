using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.BaseRepository;
using PokemonReview.Repository;

namespace PokemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepo reviewRepo;
        private readonly IReviewerRepo reviewerRepo;
        private readonly IPokemonRepo pokemonRepo;
        private readonly IMapper mapper;

        public ReviewController(IReviewRepo reviewRepo, IReviewerRepo _reviewerRepo, IPokemonRepo _pokemonRepo, IMapper mapper)
        {
            this.reviewRepo = reviewRepo;
            reviewerRepo = _reviewerRepo;
            pokemonRepo = _pokemonRepo;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetReview(int id)
        {
            if (!reviewRepo.Exist(id))
                return NotFound();

            var review = mapper.Map<ReviewDto>(reviewRepo.GetById(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(review);
        }

        [HttpGet]
        public IActionResult GetReviews()
        {
            var reviews = mapper.Map<List<ReviewDto>>(reviewRepo.GetAll());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }
        [HttpGet("Pokemon/{pokeId:int}")]
        public IActionResult GetReviewsOfPokemon(int pokeId)
        {
            var reviews = mapper.Map<List<ReviewDto>>(reviewRepo.GetReviewsOfPokemon(pokeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }
        [HttpGet("Reviewer/{reviewerId:int}")]
        public IActionResult GetReviewsOfReviewer(int reviewerId)
        {
            var reviews = mapper.Map<List<ReviewDto>>(reviewRepo.GetReviewsOfReviewer(reviewerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpPost]
        public IActionResult CreateReview([FromBody] ReviewDto createdReview)
        {
            if (createdReview == null)
                return BadRequest(ModelState);

            if (!reviewerRepo.Exist(createdReview.ReviewerId))
            {
                ModelState.AddModelError("", "Reviewer Does Not Exist");
                return BadRequest(ModelState);
            }
            if (!pokemonRepo.Exist(createdReview.PokemonId))
            {
                ModelState.AddModelError("", "Pokemon Does Not Exist");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var MappedReview = mapper.Map<Review>(createdReview);
            MappedReview.ReviewerId = createdReview.ReviewerId;
            MappedReview.PokemonId = createdReview.PokemonId;

            if (!reviewRepo.Create(MappedReview))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            createdReview.Id = MappedReview.Id;
            return StatusCode(201, createdReview);

        }

        [HttpPut("{reviewId:int}")]
        public IActionResult UpdateReview([FromRoute] int reviewId, [FromBody] ReviewDto updatedReview)
        {
            if (updatedReview == null)
                return BadRequest(ModelState);

            if (!reviewRepo.Exist(reviewId))
                return NotFound("No Review Exist");
            if (!reviewerRepo.Exist(updatedReview.ReviewerId))
            {
                ModelState.AddModelError("", "Reviewer  Does Not Exist");
                return BadRequest(ModelState);
            }
            if (!pokemonRepo.Exist(updatedReview.PokemonId))
            {
                ModelState.AddModelError("", "Pokemon Does Not Exist");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewDb = reviewRepo.GetById(reviewId);

            // Update Data
            reviewDb.Title = updatedReview.Title;
            reviewDb.Text =  updatedReview.Text;
            reviewDb.Rating = updatedReview.Rating;
            reviewDb.PokemonId =updatedReview.PokemonId;
            reviewDb.ReviewerId =updatedReview.ReviewerId;

            if (!reviewRepo.Update(reviewDb))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            updatedReview.Id = reviewDb.Id;

            return Ok(updatedReview);
        }

        [HttpDelete("{reviewId:int}")]
        public IActionResult DeleteOwner([FromRoute] int reviewId)
        {

            if (!reviewRepo.Exist(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewDb = reviewRepo.GetById(reviewId);


            if (!reviewRepo.Delete(reviewDb))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }

    }
}
