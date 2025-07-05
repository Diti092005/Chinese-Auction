using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Bll.Interfaces;
using Server.Dal.Interfaces;
using Server.Models;
using Server.Models.DTO;
using Microsoft.Extensions.Logging;

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
                _logger.LogInformation("Successfully retrieved categories");
                return Ok(categories);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "No categories found");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting categories");
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            _logger.LogInformation($"Getting category with ID {id}");
            try
            {
                var category = await _categoryService.Get(id);
                _logger.LogInformation($"Successfully retrieved category with ID {id}");
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Category with ID {id} not found");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting category by ID");
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }

        // POST api/<CategoryController>
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Post([FromBody] CategoryDTO categoryDto)
        {
            _logger.LogInformation("Adding a new category");
            try
            {
                var createdCategory = await _categoryService.Add(categoryDto);
                _logger.LogInformation($"Category with ID {createdCategory.Id} created successfully");
                return CreatedAtAction(nameof(Get), new { id = createdCategory.Id }, createdCategory);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid category data or duplicate name");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new category");
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoryDTO categoryDto)
        {
            _logger.LogInformation($"Updating category with ID {id}");
            try
            {
                await _categoryService.Update(id, categoryDto);
                _logger.LogInformation($"Category with ID {id} updated successfully");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Category with ID {id} not found");
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid category data or duplicate name");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category");
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager ")]
        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting category with ID {id}");
            try
            {
                await _categoryService.Delete(id);
                _logger.LogInformation($"Category with ID {id} deleted successfully");
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"Cannot delete category {id} because there are gifts assigned to it");
                return Conflict(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Category with ID {id} not found for deletion");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting category");
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }
    }
}