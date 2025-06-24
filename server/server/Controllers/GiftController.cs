using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using server.Bll.Interfaces;
using server.Dal.Interfaces;
using server.Models;
using server.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftsController : ControllerBase
    {
        private readonly IGiftService _giftService;
        private readonly IMapper _mapper;
        private readonly ILogger<GiftsController> _logger;

        public GiftsController(IGiftService giftService, IMapper mapper, ILogger<GiftsController> logger)
        {
            _giftService = giftService;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Getting all gifts");
            try
            {
                var gifts = await _giftService.Get();
                return Ok(gifts);
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
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"Getting gift with id {id}");
            try
            {
                var gift = await _giftService.Get(id);
                return Ok(gift);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Gift with id {id} not found");
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
        public async Task<IActionResult> Add([FromBody] GiftDTO giftDto)
        {
            _logger.LogInformation("Adding a new gift");
            try
            {
                if (giftDto == null || giftDto.CategoryId == 0 || giftDto.DonorId == 0 || giftDto.GiftName == null)
                {
                    return BadRequest("Gift data cannot be null.");
                }
                if (giftDto.Price < 10 || giftDto.Price > 100)
                {
                    return BadRequest("Price must be between 10 and 100.");
                }
               
                var existingGift = await _giftService.TitleExists(giftDto.GiftName);
                if (existingGift)
                {
                    return Conflict("Gift with this name already exists.");
                }

                var gift = _mapper.Map<Gift>(giftDto);
                await _giftService.Add(gift);
                return CreatedAtAction(nameof(Get), new { id = gift.Id }, gift);
            }
            catch(InvalidDataException ex)
            {
                _logger.LogWarning(ex, "Invalid data provided for gift");
                return BadRequest(ex.Message); 
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Operation conflict while adding gift");
                return Conflict(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding gift");
                return StatusCode(500, "server error");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] GiftDTO giftDto)
        {
            _logger.LogInformation($"Updating gift with id {id}");
            try
            {
                if (giftDto == null || giftDto.CategoryId == 0 || giftDto.DonorId == 0 || giftDto.GiftName == null )
                {
                    return BadRequest("Gift data cannot be null.");
                }
                if(giftDto.Price < 10 || giftDto.Price>100)
                {
                    return BadRequest("Price must be between 10 and 100.");
                }

                var existingGift = await _giftService.Get(id);
                if (existingGift == null)
                {
                    return NotFound($"Gift with ID {id} not found.");
                }

                var existingGiftWithSameName = await _giftService.TitleExists(giftDto.GiftName);
                if (existingGiftWithSameName && existingGift.GiftName != giftDto.GiftName)
                {
                    return Conflict("Gift with this name already exists.");
                }

                await _giftService.Update(id, giftDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Gift with id {id} not found for update");
                return NotFound(ex.Message); 
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Operation conflict while updating gift");
                return Conflict(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating gift");
                return StatusCode(500, "server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting gift with id {id}");
            try
            {
                var result = await _giftService.Delete(id);
                if (!result)
                {
                    return NotFound($"Gift with ID {id} not found.");
                }
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Gift with id {id} not found for deletion");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting gift");
                return StatusCode(500, "server error");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string giftName = null, string donorName = null, int? buyerCount = null)
        {
            _logger.LogInformation("Searching gifts");
            try
            {
                var gifts = await _giftService.Search(giftName, donorName, buyerCount);
                return Ok(gifts);
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

        [HttpGet("donor/{giftId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDonor(int giftId)
        {
            _logger.LogInformation($"Getting donor for gift id {giftId}");
            try
            {
                var donor = await _giftService.GetDonor(giftId);
                return Ok(donor);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Donor for gift id {giftId} not found");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error in GetDonor({giftId})");
                return StatusCode(500, "server error");
            }
        }

        [HttpGet("sort/price")]
        public async Task<IActionResult> SortByPrice()
        {
            _logger.LogInformation("Sorting gifts by price");
            try
            {
                var gifts = await _giftService.SortByPrice();
                return Ok(gifts);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation in SortByPrice()");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in SortByPrice()");
                return StatusCode(500, "server error");
            }
        }

        [HttpGet("sort/category")]
        public async Task<IActionResult> SortByCategory()
        {
            _logger.LogInformation("Sorting gifts by category");
            try
            {
                var gifts = await _giftService.SortByCategory();
                return Ok(gifts);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation in SortByCategory()");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in SortByCategory()");
                return StatusCode(500, "server error");
            }
        }

        [HttpPut("raffle/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Raffle(int id)
        {
            _logger.LogInformation($"Raffling gift with id {id}");
            try
            {
                await _giftService.raffle(id);
                return Ok("Raffle completed successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Gift with id {id} not found for raffle");
                return NotFound(ex.Message); 
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation in Raffle()");
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in Raffle()");
                return StatusCode(500, "server error"); 
            }
        }
    }
}
