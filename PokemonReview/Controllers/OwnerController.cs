using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.BaseRepository;
using PokemonReview.Repository;

namespace PokemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepo ownerRepo;
        private readonly IPokemonRepo pokemonRepo;
        private readonly ICountryRepo countryRepo;
        private readonly IMapper mapper;

        public OwnerController(IOwnerRepo _ownerRepo, IPokemonRepo _pokemonRepo, ICountryRepo _countryRepo, IMapper mapper)
        {
            this.ownerRepo = _ownerRepo;
            this.mapper = mapper;
            this.pokemonRepo = _pokemonRepo;
            countryRepo = _countryRepo;
        }


        [HttpGet("{id:int}")]
        public IActionResult GetOwner(int id)
        {
            if (!ownerRepo.Exist(id))
                return NotFound();

            var owner = mapper.Map<OwnerDto>(ownerRepo.GetById(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
           
            return Ok(owner);
        }
        [HttpGet]
        public IActionResult GetOwners()
        {

            var owners = mapper.Map<List<OwnerDto>>(ownerRepo.GetAll());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);
        }


        [HttpGet("Pokemon/{pokeId:int}")]
        public IActionResult GetOwnersOfPokemon(int pokeId)
        {
            if (!pokemonRepo.Exist(pokeId))
                return NotFound();

            var owners = mapper.Map<List<OwnerDto>>(ownerRepo.GetOwnersOfPokemon(pokeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);

        }

        [HttpGet("Country/{countryId:int}")]
        public IActionResult GetOwnersByCountry(int countryId)
        {
            if(!countryRepo.Exist(countryId))
                return NotFound();

            var owners = mapper.Map<List<OwnerDto>>(ownerRepo.GetOwnersOfCountry(countryId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owners);

        }

        [HttpPost("{countryId:int}")]
        public IActionResult CreateOwner([FromBody]OwnerDto createdOwner)
        {
            if (createdOwner == null)
                return BadRequest(ModelState);
            if (!countryRepo.Exist(createdOwner.CountryId))
            {
                ModelState.AddModelError("", "Country Not Exist");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var MappedOwner = mapper.Map<Owner>(createdOwner);
            MappedOwner.CountryId = createdOwner.CountryId;

            if (!ownerRepo.Create(MappedOwner))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            createdOwner.Id = MappedOwner.Id;
            return StatusCode(201, createdOwner);

        }

        [HttpPut("{ownerId:int}")]
        public IActionResult UpdateCategory([FromRoute] int ownerId, [FromBody] OwnerDto updatedOwner)
        {
            if (updatedOwner == null)
                return BadRequest(ModelState);

            if (!ownerRepo.Exist(ownerId))
                return NotFound();
            if (!countryRepo.Exist(updatedOwner.CountryId))
            {
                ModelState.AddModelError("", "Country Does Not Exist");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerDb = ownerRepo.GetById(ownerId);

            // Update Data
            ownerDb.FirstName = updatedOwner.FirstName;
            ownerDb.LastName = updatedOwner.LastName;
            ownerDb.CountryId = updatedOwner.CountryId;
            ownerDb.Gym = updatedOwner.Gym;

            if (!ownerRepo.Update(ownerDb))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            updatedOwner.Id = ownerDb.Id;

            return Ok(updatedOwner);
        }

        [HttpDelete("{owenerId:int}")]
        public IActionResult DeleteOwner([FromRoute] int owenerId)
        {

            if (!ownerRepo.Exist(owenerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var owenerDb = ownerRepo.GetById(owenerId);


            if (!ownerRepo.Delete(owenerDb))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }



    }
}
