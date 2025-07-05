using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Dal.Interfaces;
using Server.Models;
using Server.Models.DTO;
using Server.Dal;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Server.Dal
{
    public class TicketDal: ITicketDal
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserDal _userDal;
        private readonly IMapper _mapper;
        private readonly ILogger<TicketDal> _logger;

        public TicketDal(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IUserDal userDal, IMapper mapper, ILogger<TicketDal> logger)
        {
            this._dbContext = dbContext;
            this._httpContextAccessor = httpContextAccessor;
            this._userDal = userDal;
            this._mapper = mapper;
            this._logger = logger;
        }
        public async Task<List<Ticket>> Get()
        {
            _logger.LogInformation("Getting all tickets");
            var tickets = await _dbContext.Tickets
                .Where(t => t.Status != TicketStatus.Pending)
                .Include(t => t.Gift)
                .Include(t => t.User)
                .ToListAsync();
            if (tickets == null || !tickets.Any())
            {
                _logger.LogWarning("No tickets found");
                throw new InvalidOperationException("No tickets found.");
            }
            _logger.LogInformation($"Returned {tickets.Count} tickets");
            return tickets;
        }
        public async Task<List<Ticket>> GetByUserPaid()
        {
            var user = await _userDal.GetUserFromToken();
            var tickets = await _dbContext.Tickets
                .Where(t => t.UserId == user.Id && t.Status != TicketStatus.Pending)
                .Include(t => t.Gift)
                .ToListAsync();
            if (tickets == null)
            {
                throw new InvalidOperationException("No tickets found for this user.");
            }
            var ticketDtos = _mapper.Map<List<Ticket>>(tickets);
            return ticketDtos;
        }
        public async Task<List<Ticket>> GetByUserPending()
        {
            var user = await _userDal.GetUserFromToken();
            var tickets = await _dbContext.Tickets
                .Where(t => t.UserId == user.Id && t.Status == TicketStatus.Pending)
                .Include(t => t.Gift)
                .ToListAsync();
            if (tickets == null)
            {
                throw new InvalidOperationException("No tickets found for this user.");
            }
            return tickets;
        }
        public async Task<Ticket> Get(int id)
        {
            var user = await _userDal.GetUserFromToken();

            var ticket = await _dbContext.Tickets.Include(t => t.Gift).FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {id} not found.");
            }
            if (ticket.UserId != user.Id)
            {
                throw new UnauthorizedAccessException("You are not authorized to view this ticket.");
            }
            return ticket;
        }

        public async Task<List<Ticket>> GetByGiftId(int giftId)
        {
            var tickets = await _dbContext.Tickets
                .Where(t => t.GiftId == giftId)
                .Include(t => t.Gift)
                .Include(t => t.User)
                .ToListAsync();

            if (tickets == null)
            {
                return new List<Ticket>();
            }

            return tickets;
        }

        public async Task Add(Ticket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException(nameof(ticket), "Ticket cannot be null.");
            }
            var existGift = await _dbContext.Gifts.FindAsync(ticket.GiftId);
            if (existGift == null)
            {
                throw new InvalidDataException($"Gift with ID {ticket.GiftId} not found.");
            }
            _dbContext.Tickets.Add(ticket);
            await _dbContext.SaveChangesAsync();
        }

        public async Task pay(int [] ids)
        {
            var user = await _userDal.GetUserFromToken();
            for (int i = 0; i < ids.Length; i++)
            {
                var ticket = await _dbContext.Tickets.FindAsync(ids[i]);
                Console.WriteLine("id"+ids[i]);
                if (ticket == null)
                {
                    throw new KeyNotFoundException($"Ticket with ID {ids[i]} not found.");
                }
                if (ticket.UserId != user.Id)
                {
                    throw new UnauthorizedAccessException("You are not authorized to delete this ticket.");
                }
                if (ticket.Status != TicketStatus.Pending)
                {
                    throw new InvalidOperationException($"Ticket with ID {ids[i]} is not in a state that can be paid.");
                }
                ticket.Status = TicketStatus.Paid;
            }
            
            await _dbContext.SaveChangesAsync();
        }

        public async Task Win(int id)
        {
            var ticket = await _dbContext.Tickets.FindAsync(id);
            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {id} not found.");
            }
            if (ticket.Status != TicketStatus.Paid)
            {
                throw new InvalidOperationException($"Ticket with ID {id} is not in a state that can be marked as won.");
            }
            ticket.Status = TicketStatus.Win;
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var usernameFromToken = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(usernameFromToken))
            {
                throw new UnauthorizedAccessException("Username not found in token.");
            }
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == usernameFromToken);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }
            var ticket = await _dbContext.Tickets.FindAsync(id);
            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {id} not found.");
            }
            if (ticket.UserId != user.Id)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this ticket.");
            }
            
            if (ticket.Status != TicketStatus.Pending)
            {
                throw new InvalidOperationException($"Ticket with ID {id} is not in a state that can be deleted.");
            }
            _dbContext.Tickets.Remove(ticket);
            await _dbContext.SaveChangesAsync();
        }
    }
}
