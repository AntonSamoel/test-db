using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.BaseRepository;
using PokemonReview.Repository;

namespace PokemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepo reviewerRepo;
        private readonly IReviewRepo reviewRepo;
        private readonly IMapper mapper;

        public ReviewerController(IReviewerRepo reviewerRepo, IReviewRepo reviewRepo, IMapper mapper)
        {
            this.reviewerRepo = reviewerRepo;
            this.reviewRepo = reviewRepo;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetReviewer(int id)
        {
            if (!reviewerRepo.Exist(id))
                return NotFound();

            var reviewer = mapper.Map<ReviewerDto>(reviewerRepo.GetById(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewer);
        }

        [HttpGet]
        public IActionResult GetReviewers()
        {
            var reviewers = mapper.Map<List<ReviewerDto>>(reviewerRepo.GetAll());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewers);
        }

        [HttpPost]
        public IActionResult CreateReviewer([FromBody] ReviewerDto createdReviewer)
        {
            if (createdReviewer == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var MappedReviewer = mapper.Map<Reviewer>(createdReviewer);

            if (!reviewerRepo.Create(MappedReviewer))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            createdReviewer.Id = MappedReviewer.Id;
            return StatusCode(201, createdReviewer);

        }

        [HttpPut("{reviewerId:int}")]
        public IActionResult UpdateCategory([FromRoute] int reviewerId, [FromBody] ReviewerDto updatedReviewer)
        {
            if (updatedReviewer == null)
                return BadRequest(ModelState);

            if (!reviewerRepo.Exist(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerDb = reviewerRepo.GetById(reviewerId);

            // Update Name
            reviewerDb.FirstName = updatedReviewer.FirstName;
            reviewerDb.LastName = updatedReviewer.LastName;

            if (!reviewerRepo.Update(reviewerDb))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            updatedReviewer.Id = reviewerDb.Id;

            return Ok(updatedReviewer);
        }

        [HttpDelete("{reviewerId:int}")]
        public IActionResult DeleteReviewer([FromRoute] int reviewerId)
        {

            if (!reviewerRepo.Exist(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (reviewRepo.GetReviewsOfReviewer(reviewerId).Count > 0)
            {
                ModelState.AddModelError("Forign Key Constrain", "Cannot Delete the reviewer as it refrences by review(s)");
                return BadRequest(ModelState);
            }

            var reviewerDb = reviewerRepo.GetById(reviewerId);


            if (!reviewerRepo.Delete(reviewerDb))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }


    }
}
