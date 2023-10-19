using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.BaseRepository;
using PokemonReview.Dto;

namespace PokemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo categoryRepo;
        private readonly IPokemonRepo pokemonRepo;
        private readonly IMapper mapper;

        public CategoryController(ICategoryRepo _categoryRepo, IPokemonRepo _pokemonRepo, IMapper mapper)
        {
            this.categoryRepo = _categoryRepo;
            pokemonRepo = _pokemonRepo;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetCategory(int id)
        {
            if (!categoryRepo.Exist(id))
                return NotFound();

            var category = mapper.Map<CategoryDto>(categoryRepo.GetById(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }
        [HttpGet]
        public IActionResult GetCategories()
        {

            var categories = mapper.Map<List<CategoryDto>>(categoryRepo.GetAll());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categories);
        }

        [HttpGet("Pokemon/{categoryId:int}")]
        public IActionResult GetPokemonsByCategoryId(int categoryId)
        {
            if (!categoryRepo.Exist(categoryId))
                return NotFound();

            var pokemons = mapper.Map<List<PokemonDto>>(categoryRepo.GetPokemonsByCategotyId(categoryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryDto createdCategory)
        {
            if (createdCategory == null)
                return BadRequest(ModelState);

            var category = categoryRepo.GetAll()
                .Where(c => c.Name.Trim().ToLower() == createdCategory.Name.Trim().ToLower()).FirstOrDefault();

            if(category != null)
            {
                ModelState.AddModelError("", "Category Already Exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var MappedCategory = mapper.Map<Category>(createdCategory);

            if (!categoryRepo.Create(MappedCategory))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            createdCategory.Id = MappedCategory.Id;
            return StatusCode(201,createdCategory);

        }

        [HttpPut("{categoryId:int}")]
        public IActionResult UpdateCategory([FromRoute] int categoryId, [FromBody] CategoryDto updatedCategory)
        {
            if(updatedCategory == null)
                return BadRequest(ModelState);

            if (!categoryRepo.Exist(categoryId))
                return NotFound();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryDb = categoryRepo.GetById(categoryId);

            // Update Name
            categoryDb.Name = updatedCategory.Name;

            if (!categoryRepo.Update(categoryDb))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500,ModelState);
            }

            updatedCategory.Id = categoryDb.Id;

            return Ok(updatedCategory);
        }

        [HttpDelete("{categoryId:int}")]
        public IActionResult UpdateCategory([FromRoute] int categoryId)
        {
 
            if (!categoryRepo.Exist(categoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (pokemonRepo.GetPokemonsOfCategory(categoryId).Count>0)
            {
                ModelState.AddModelError("Forign Key Constrain", "Cannot Delete the category as it refrences by pokemons");
                return BadRequest(ModelState);
            }

            var categoryDb = categoryRepo.GetById(categoryId);


            if (!categoryRepo.Delete(categoryDb))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }

    }
}
