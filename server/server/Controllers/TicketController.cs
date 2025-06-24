using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IMapper _mapper;
        private readonly IUserDal _userDal;
        private readonly ILogger<TicketController> _logger;

        public TicketController(ITicketService ticketService, IMapper mapper, IUserDal userDal, ILogger<TicketController> logger)
        {
            this._ticketService = ticketService;
            this._mapper = mapper;
            this._userDal = userDal;
            this._logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Getting all tickets");
            try
            {
                var tickets = await _ticketService.Get();
                return Ok(tickets);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation in Get()");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in Get()");
                return StatusCode(500, new { message = "An unexpected error occurred."});
            }
        }

        [HttpGet("paid")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetByUserPaid()
        {
            _logger.LogInformation("Getting paid tickets for user");
            try
            {
                var tickets = await _ticketService.GetByUserPaid();
                return Ok(tickets);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access in GetByUserPaid()");
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation in GetByUserPaid()");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetByUserPaid()");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("pending")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetByUserPending()
        {
            _logger.LogInformation("Getting pending tickets for user");
            try
            {
                var tickets = await _ticketService.GetByUserPending();
                return Ok(tickets);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access in GetByUserPending()");
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation in GetByUserPending()");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetByUserPending()");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"Getting ticket with id {id}");
            try
            {
                var ticket = await _ticketService.Get(id);
                return Ok(ticket);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, $"Unauthorized access to ticket id {id}");
                return Unauthorized(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Ticket not found with id {id}");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error while getting ticket id {id}");
                return StatusCode(500, new { message = "An unexpected error occurred."});
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] TicketDTO ticketDto)
        {
            _logger.LogInformation("Adding a new ticket");
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
                _logger.LogWarning(ex, "Argument null exception in Add()");
                return BadRequest(new { message = ex.Message });
            }
            catch(InvalidDataException ex)
            {
                _logger.LogWarning(ex, "Invalid data in Add()");
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access in Add()");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in Add()");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpPut("pay/{id}")]
        [Authorize]
        public async Task<IActionResult> Pay(int id)
        {
            _logger.LogInformation($"Processing payment for ticket id {id}");
            try
            {
                await _ticketService.pay(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Ticket not found for payment id {id}");
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, $"Unauthorized payment attempt for ticket id {id}");
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"Invalid operation for payment id {id}");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error while processing payment id {id}");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting ticket id {id}");
            try
            {
                await _ticketService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Ticket not found for deletion id {id}");
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, $"Unauthorized deletion attempt for ticket id {id}");
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"Invalid operation for deletion id {id}");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error while deleting ticket id {id}");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
