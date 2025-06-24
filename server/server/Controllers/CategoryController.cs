using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using server.Bll.Interfaces;
using server.Dal.Interfaces;
using server.Models;
using server.Models.DTO;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, IMapper mapper, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            _logger.LogInformation("Getting all categories");
            try
            {
                var categories = await _categoryService.Get();
                return Ok(categories);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation in Get()");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in Get()");
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            _logger.LogInformation($"Getting category with id {id}");
            try
            {
                var category = await _categoryService.Get(id);
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Category with id {id} not found");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error in Get({id})");
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }

        // POST api/<CategoryController>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Post([FromBody] CategoryDTO categoryDto)
        {
            _logger.LogInformation("Creating new category");
            try
            {
                var category = _mapper.Map<Category>(categoryDto);
                var duplicate = await _categoryService.NameExist(category.Name);
                if (duplicate)
                {
                    _logger.LogWarning($"Category with name {category.Name} already exists");
                    return Conflict($"Category with name {category.Name} already exists.");
                }

                await _categoryService.Add(category);
                _logger.LogInformation($"Category {category.Name} created successfully");
                return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Argument null in Post()");
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in Post()");
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoryDTO categoryDto)
        {
            _logger.LogInformation($"Updating category with id {id}");
            try
            {
                var category = _mapper.Map<Category>(categoryDto);
                var existingCategory = await _categoryService.Get(id);
                if (existingCategory == null)
                {
                    _logger.LogWarning($"Category with ID {id} not found for update");
                    return NotFound($"Category with ID {id} not found.");
                }

                var duplicate = await _categoryService.NameExist(category.Name);
                if (duplicate)
                {
                    _logger.LogWarning($"Category with name {category.Name} already exists");
                    return Conflict($"Category with name {category.Name} already exists.");
                }

                await _categoryService.Update(id, category);
                _logger.LogInformation($"Category with id {id} updated successfully");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Category with id {id} not found in Put()");
                return NotFound(ex.Message); 
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Argument null in Put()");
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error in Put({id})");
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting category with id {id}");
            try
            {
                await _categoryService.Delete(id);
                _logger.LogInformation($"Category with id {id} deleted successfully");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Category with id {id} not found in Delete()");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error in Delete({id})");
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }
    }
}