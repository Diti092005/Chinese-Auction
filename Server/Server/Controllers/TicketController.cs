using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Getting all tickets");
            try
            {
                var tickets = await _ticketService.Get();
                _logger.LogInformation("Successfully retrieved tickets");
                return Ok(tickets);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "No tickets found");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting tickets");
                return StatusCode(500, new { message = "An unexpected error occurred."});
            }
        }

        [HttpGet("paid")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetByUserPaid()
        {
            _logger.LogInformation("Getting paid tickets for the user");
            try
            {
                var tickets = await _ticketService.GetByUserPaid();
                _logger.LogInformation("Successfully retrieved paid tickets for the user");
                return Ok(tickets);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt");
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "No paid tickets found for the user");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paid tickets for the user");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("pending")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetByUserPending()
        {
            _logger.LogInformation("Getting pending tickets for the user");
            try
            {
                var tickets = await _ticketService.GetByUserPending();
                _logger.LogInformation("Successfully retrieved pending tickets for the user");
                return Ok(tickets);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt");
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "No pending tickets found for the user");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting pending tickets for the user");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User, Manager")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"Getting ticket with ID {id}");
            try
            {
                var ticket = await _ticketService.Get(id);
                _logger.LogInformation($"Successfully retrieved ticket with ID {id}");
                return Ok(ticket);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt");
                return Unauthorized(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Ticket with ID {id} not found");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting ticket with ID {id}");
                return StatusCode(500, new { message = "An unexpected error occurred."});
            }
        }


        [HttpGet("byGift/{giftId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetByGiftId(int giftId)
        {
            _logger.LogInformation($"Getting tickets by Gift ID {giftId}");
            try
            {
                var tickets = await _ticketService.GetByGiftId(giftId);
                _logger.LogInformation($"Successfully retrieved tickets by Gift ID {giftId}");
                return Ok(tickets);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt");
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"No tickets found for Gift ID {giftId}");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting tickets by Gift ID {giftId}");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] TicketDTO ticketDto)
        {
            _logger.LogInformation("Adding a new ticket");
            try
            {
                var user = await _userDal.GetUserFromToken();
                await _ticketService.Add(ticketDto, user.Id);
                _logger.LogInformation($"Successfully added a new ticket");
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Ticket data is null");
                return BadRequest(new { message = ex.Message });
            }
            catch(InvalidDataException ex)
            {
                _logger.LogWarning(ex, "Invalid data for ticket");
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new ticket");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpPut("pay")]
        [Authorize]
        public async Task<IActionResult> Pay(int[] ids)
        {
            _logger.LogInformation($"Processing payment for tickets");
            try
            {
                await _ticketService.pay(ids);
                _logger.LogInformation($"Successfully processed payment for tickets");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Tickets not found for payment");
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt for payment");
                return Unauthorized(new { message = ex.Message });
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"Invalid operation for payment of tickets");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while processing payment for tickets");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting ticket with ID {id}");
            try
            {
                await _ticketService.Delete(id);
                _logger.LogInformation($"Successfully deleted ticket with ID {id}");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Ticket with ID {id} not found for deletion");
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt for deletion");
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"Invalid operation for deletion of ticket ID {id}");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting ticket with ID {id}");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
