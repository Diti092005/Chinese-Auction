using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.BLL.Intefaces;
using server.DAL.intefaces;
using server.Models;
using server.Models.DTO;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IMapper _mapper;
        private readonly IUserDal _userDal;

        public TicketController(ITicketService ticketService, IMapper mapper, IUserDal userDal)
        {
            this._ticketService = ticketService;
            this._mapper = mapper;
            this._userDal = userDal;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var tickets = await _ticketService.Get();
                return Ok(tickets);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("paid")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetByUserPaid()
        {
            try
            {
                var tickets = await _ticketService.GetByUserPaid();
                return Ok(tickets);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("pending")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetByUserPending()
        {
            try
            {
                var tickets = await _ticketService.GetByUserPending();
                return Ok(tickets);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var ticket = await _ticketService.Get(id);
                return Ok(ticket);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] TicketDto ticketDto)
        {
            try
            {
                var ticket = _mapper.Map<Ticket>(ticketDto);
                var user = await _userDal.GetUserFromToken();
                ticket.UserId = user.Id;
                await _ticketService.Add(ticket);
                return CreatedAtAction(nameof(Get), new { id = ticket.Id }, ticket);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpPut("pay/{id}")]
        [Authorize]
        public async Task<IActionResult> Pay(int id)
        {
            try
            {
                await _ticketService.pay(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _ticketService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
        [HttpGet("byGift/{giftId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByGiftId(int giftId)
        {
            try
            {
                var tickets = await _ticketService.GetByGiftId(giftId);
                return Ok(tickets);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

    }
}
