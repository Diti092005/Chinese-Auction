using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using server.Bll.Interfaces;
using server.Dal.Interfaces;
using server.Models;
using server.Models.DTO;
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Getting all donors");
            try
            {
                var donors = await _donorService.Get();
                return Ok(donors);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation in Get()");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in Get()");
                return StatusCode(500, "server error");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"Getting donor with id {id}");
            try
            {
                var donor = await _donorService.Get(id);
                return Ok(donor);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Donor with id {id} not found");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error in Get({id})");
                return StatusCode(500, "server error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] DonorDTO donorDto)
        {
            _logger.LogInformation("Creating new donor");
            try
            {
                if (donorDto == null )
                {
                    _logger.LogWarning("Donor data cannot be null");
                    return BadRequest("Donor data cannot be null.");
                }

                var donor = _mapper.Map<Donor>(donorDto);
                await _donorService.Add(donor);
                _logger.LogInformation($"Donor {donor.Name} created successfully");
                return CreatedAtAction(nameof(Get), new { id = donor.Id }, donor);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Argument null in Add()");
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in Add()");
                return StatusCode(500, "server error");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] DonorDTO donorDto)
        {
            _logger.LogInformation($"Updating donor with id {id}");
            try
            {
                if (donorDto == null )
                {
                    _logger.LogWarning("Donor data cannot be null");
                    return BadRequest("Donor data cannot be null.");
                }

                var existingDonor = await _donorService.Get(id);
                if (existingDonor == null)
                {
                    _logger.LogWarning($"Donor with ID {id} not found for update");
                    return NotFound($"Donor with ID {id} not found.");
                }

                await _donorService.Update(id, donorDto);
                _logger.LogInformation($"Donor with id {id} updated successfully");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Donor with id {id} not found in Update()");
                return NotFound(ex.Message); 
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Argument null in Update()");
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error in Update({id})");
                return StatusCode(500, "server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting donor with id {id}");
            try
            {
                var existingDonor = await _donorService.Get(id);
                if (existingDonor == null)
                {
                    _logger.LogWarning($"Donor with ID {id} not found for delete");
                    return NotFound($"Donor with ID {id} not found.");
                }

                await _donorService.Delete(id);
                _logger.LogInformation($"Donor with id {id} deleted successfully");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Donor with id {id} not found in Delete()");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error in Delete({id})");
                return StatusCode(500, "server error");
            }
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Search(string name = null, string email = null, string giftName = null)
        {
            _logger.LogInformation($"Searching donors by name: {name}, email: {email}, giftName: {giftName}");
            try
            {
                var donors = await _donorService.Search(name, email, giftName);
                return Ok(donors);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation in Search()");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in Search()");
                return StatusCode(500, "server error");
            }
        }
        [HttpGet("{id}/countOfGifts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CountOfGifts(int id)
        {
            _logger.LogInformation($"Counting gifts for donor id {id}");
            try
            {
                var count = await _donorService.CountOfGifts(id);
                return Ok(count);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Donor with id {id} not found in CountOfGifts()");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error in CountOfGifts({id})");
                return StatusCode(500, "server error");
            }
        }
    }
}
