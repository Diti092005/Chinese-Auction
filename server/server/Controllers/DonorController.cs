using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using server.BLL.Intefaces;
using server.Models.DTO;
using server.Models;
using Microsoft.AspNetCore.Authorization;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService _donorService;
        private readonly IMapper _mapper;
        public DonorController(IDonorService donorService, IMapper mapper)
        {
            _donorService = donorService;
            _mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var donors = await _donorService.Get();
                return Ok(donors);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var donor = await _donorService.Get(id);
                return Ok(donor);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] DonorDto donorDto)
        {
            try
            {
                if (donorDto == null )
                {
                    return BadRequest("Donor data cannot be null.");
                }

                var donor = _mapper.Map<Donor>(donorDto);
                await _donorService.Add(donor);
                return CreatedAtAction(nameof(Get), new { id = donor.Id }, donor);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] DonorDto donorDto)
        {
            try
            {
                if (donorDto == null )
                {
                    return BadRequest("Donor data cannot be null.");
                }

                var existingDonor = await _donorService.Get(id);
                if (existingDonor == null)
                {
                    return NotFound($"Donor with ID {id} not found.");
                }

                await _donorService.Update(id, donorDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingDonor = await _donorService.Get(id);
                if (existingDonor == null)
                {
                    return NotFound($"Donor with ID {id} not found.");
                }

                await _donorService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error");
            }
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Search(string name = null, string email = null, string giftName = null)
        {
            try
            {
                var donors = await _donorService.Search(name, email, giftName);
                return Ok(donors);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error");
            }
        }
    }
}
