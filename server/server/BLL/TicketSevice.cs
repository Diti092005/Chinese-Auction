using Microsoft.Extensions.Logging;
using server.Bll.Interfaces;
using server.Dal.Interfaces;
using server.Models;
using server.Models.DTO;

namespace server.Bll
{
    public class TicketSevice : ITicketService
    {
        private readonly ITicketDal  _ticketDal;
        private readonly IGiftDal _giftDal;
        private readonly ILogger<TicketSevice> _logger;

        public TicketSevice(ITicketDal ticketDal, IGiftDal giftDal, ILogger<TicketSevice> logger)
        {
            _ticketDal = ticketDal;
            _giftDal = giftDal;
            _logger = logger;
        }
        public async Task<IEnumerable<Ticket>> Get()
        {
            _logger.LogInformation("Getting all tickets");
            try
            {
                return await _ticketDal.Get();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all tickets");
                throw;
            }
        }

        public async Task<IEnumerable<Ticket>> GetByUserPaid()
        {
            _logger.LogInformation("Getting paid tickets for user");
            try
            {
                return await _ticketDal.GetByUserPaid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paid tickets for user");
                throw;
            }
        }
        public async Task<IEnumerable<Ticket>> GetByUserPending()
        {
            _logger.LogInformation("Getting pending tickets for user");
            try
            {
                return await _ticketDal.GetByUserPending();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending tickets for user");
                throw;
            }
        }
        public async Task<Ticket> Get(int id)
        {
            _logger.LogInformation($"Getting ticket with id {id}");
            try
            {
                return await _ticketDal.Get(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting ticket with id {id}");
                throw;
            }
        }
        public async Task Add(Ticket ticket)
        {
            _logger.LogInformation($"Adding ticket for user {ticket.UserId}");
            try
            {
                await _ticketDal.Add(ticket);
                _logger.LogInformation($"Ticket for user {ticket.UserId} added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding ticket for user {ticket.UserId}");
                throw;
            }
        }
        public async Task pay(int id)
        {
            _logger.LogInformation($"Paying for ticket with id {id}");
            try
            {
                await _ticketDal.pay(id);
                _logger.LogInformation($"Ticket with id {id} paid successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error paying for ticket with id {id}");
                throw;
            }
        }
        public async Task Delete(int id)
        {
            _logger.LogInformation($"Deleting ticket with id {id}");
            try
            {
                await _ticketDal.Delete(id);
                _logger.LogInformation($"Ticket with id {id} deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting ticket with id {id}");
                throw;
            }
        }
        
    }
}
