using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.BaseRepository;
using PokemonReview.Dto;
using PokemonReview.Models;
using PokemonReview.Repository;

namespace PokemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepo pokemonRepo;
        private readonly IOwnerRepo ownerRepo;
        private readonly ICategoryRepo categoryRepo;
        private readonly IMapper mapper;

        public PokemonController(IPokemonRepo pokemonRepo,IOwnerRepo ownerRepo, ICategoryRepo _categoryRepo, IMapper mapper)
        {
            this.pokemonRepo = pokemonRepo;
            this.ownerRepo = ownerRepo;
            categoryRepo = _categoryRepo;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetPokemon(int id)
        {
            if (!pokemonRepo.Exist(id))
                return NotFound();

            var pokemon = mapper.Map<PokemonDto>(pokemonRepo.GetById(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = mapper.Map<List<PokemonDto>>(pokemonRepo.GetAll().ToList());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);

        }
        [HttpGet("{id:int}/rate")]
        [ProducesResponseType(200,Type =typeof(decimal))]
        public IActionResult GetPokemonRate(int id)
        {
            if (!pokemonRepo.Exist(id))
                return NotFound();

            var rate = pokemonRepo.GetRate(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rate);
        }

        [HttpGet("Owner/{ownerId:int}")]
        public IActionResult GetPokemonsOfOwner(int ownerId)
        {
            if (!ownerRepo.Exist(ownerId))
                return NotFound();

            var pokemons = mapper.Map<List<PokemonDto>>(pokemonRepo.GetPokemonsOfOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);

        }

        [HttpPost]
        public IActionResult CreatePokemon([FromQuery] int owenerId, [FromQuery] int categoryId,[FromBody] PokemonDto createdPokemon)
        {
            if (createdPokemon == null)
                return BadRequest(ModelState);

            if (!categoryRepo.Exist(categoryId))
            {
                ModelState.AddModelError("", "Category Does Not Exist");
                return BadRequest(ModelState);
            }
            if (!ownerRepo.Exist(owenerId))
            {
                ModelState.AddModelError("", "Owner Does Not Exist");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var MappedPokemon = mapper.Map<Pokemon>(createdPokemon);

            if (!pokemonRepo.CreatePokemon(owenerId,categoryId,MappedPokemon))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            createdPokemon.Id = MappedPokemon.Id;
            return StatusCode(201, createdPokemon);

        }


        [HttpPut("{pokeId:int}")]
        public IActionResult UpdatePokemon([FromRoute] int pokeId, [FromBody] PokemonDto updatedPokemon)
        {
            if (updatedPokemon == null)
                return BadRequest(ModelState);

            if (!pokemonRepo.Exist(pokeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokeDb = pokemonRepo.GetById(pokeId);

            // Update Name
            pokeDb.Name = updatedPokemon.Name;
            pokeDb.BirthDate = updatedPokemon.BirthDate;

            if (!pokemonRepo.UpdatePokemon(pokeDb))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            updatedPokemon.Id = pokeDb.Id;

            return Ok(updatedPokemon);
        }

        [HttpDelete("{pokeId:int}")]
        public IActionResult DeletePokemon([FromRoute] int pokeId)
        {

            if (!pokemonRepo.Exist(pokeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokeDb = pokemonRepo.GetById(pokeId);

            

            if (!pokemonRepo.DeletePokemon(pokeDb))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }

    }
}
