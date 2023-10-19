using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.BaseRepository;
using PokemonReview.Dto;
using PokemonReview.Repository;

namespace PokemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepo countryRepo;
        private readonly IOwnerRepo ownerRepo;
        private readonly IMapper mapper;

        public CountryController(ICountryRepo _countryRepo, IOwnerRepo _ownerRepo, IMapper mapper)
        {
            this.countryRepo = _countryRepo;
            ownerRepo = _ownerRepo;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetCountry(int id)
        {
            if (!countryRepo.Exist(id))
                return NotFound();

            var country = mapper.Map<CountryDto>(countryRepo.GetById(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }
        [HttpGet]
        public IActionResult GetCountries()
        {

            var countries = mapper.Map<List<CountryDto>>(countryRepo.GetAll());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countries);
        }

        [HttpGet("Owner/{owenerId:int}")]
        public IActionResult GetCountryOfOwner(int owenerId)
        {
            if (!ownerRepo.Exist(owenerId))
                return NotFound();

            var country = mapper.Map<CountryDto>(countryRepo.GetCountryOfOwner(owenerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpPost]
        public IActionResult CreateCategory(CountryDto createdCountry)
        {
            if (createdCountry == null)
                return BadRequest(ModelState);

            var country = countryRepo.GetAll()
                .Where(c => c.Name.Trim().ToLower() == createdCountry.Name.Trim().ToLower()).FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", "Country Already Exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var MappedCountry= mapper.Map<Country>(createdCountry);

            if (!countryRepo.Create(MappedCountry))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            createdCountry.Id = MappedCountry.Id;
            return StatusCode(201, createdCountry);

        }

        [HttpPut("{countryId:int}")]
        public IActionResult UpdateCategory([FromRoute] int countryId, [FromBody] CategoryDto updatedCountry)
        {
            if (updatedCountry == null)
                return BadRequest(ModelState);

            if (!countryRepo.Exist(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryDb = countryRepo.GetById(countryId);

            // Update Name
            countryDb.Name = updatedCountry.Name;

            if (!countryRepo.Update(countryDb))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            updatedCountry.Id = countryDb.Id;

            return Ok(updatedCountry);
        }

        [HttpDelete("{countryId:int}")]
        public IActionResult DeleteCountry([FromRoute] int countryId)
        {

            if (!countryRepo.Exist(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (ownerRepo.GetOwnersOfCountry(countryId).Count > 0)
            {
                ModelState.AddModelError("Forign Key Constrain", "Cannot Delete the country as it refrences by owner(s)");
                return BadRequest(ModelState);
            }

            var categoryDb = countryRepo.GetById(countryId);


            if (!countryRepo.Delete(categoryDb))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }

    }
}
