using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Bll.Interfaces;
using Server.Dal.Interfaces;
using Server.Models;
using Server.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService _donorService;
        private readonly IMapper _mapper;
        private readonly ILogger<DonorController> _logger;
        public DonorController(IDonorService donorService, IMapper mapper, ILogger<DonorController> logger)
        {
            _donorService = donorService;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Getting all donors");
            try
            {
                var donors = await _donorService.Get();
                _logger.LogInformation("Successfully retrieved donors");
                return Ok(donors);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "No donors found");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting donors");
                return StatusCode(500, "server error");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"Getting donor with ID {id}");
            try
            {
                var donor = await _donorService.Get(id);
                _logger.LogInformation($"Successfully retrieved donor with ID {id}");
                return Ok(donor);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Donor with ID {id} not found");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting donor with ID {id}");
                return StatusCode(500, "server error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Add([FromBody] DonorDTO donorDto)
        {
            _logger.LogInformation("Adding a new donor");
            try
            {
                await _donorService.Add(donorDto);
                _logger.LogInformation($"Successfully added donor");
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Donor data is null");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new donor");
                return StatusCode(500, "server error");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles ="Manager")]
        public async Task<IActionResult> Update(int id, [FromBody] DonorDTO donorDto)
        {
            _logger.LogInformation($"Updating donor with ID {id}");
            try
            {
                await _donorService.Update(id, donorDto);
                _logger.LogInformation($"Successfully updated donor with ID {id}");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Donor with ID {id} not found");
                return NotFound(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Donor data is null");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating donor with ID {id}");
                return StatusCode(500, "server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting donor with ID {id}");
            try
            {
                await _donorService.Delete(id);
                _logger.LogInformation($"Successfully deleted donor with ID {id}");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Donor with ID {id} not found");
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting donor with ID {id}");
                return StatusCode(500, "server error");
            }
        }

        [HttpGet("search")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Search(string? name = null, string? email = null, string? giftName = null)
        {
            _logger.LogInformation("Searching donors");
            try
            {
                var donors = await _donorService.Search(name ?? string.Empty, email ?? string.Empty, giftName ?? string.Empty);
                _logger.LogInformation("Successfully retrieved search results for donors");
                return Ok(donors);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "No donors found matching the search criteria");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for donors");
                return StatusCode(500, "server error");
            }
        }
    }
}
