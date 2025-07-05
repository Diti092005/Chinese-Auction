using Server.Models;
using Server.Models.DTO;

namespace Server.Bll.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> Get();
        Task<IEnumerable<Ticket>> GetByUserPaid();
        Task<IEnumerable<Ticket>> GetByUserPending();
        Task<IEnumerable<Ticket>> GetByGiftId(int giftId);

        Task<Ticket> Get(int id);
        Task Add(Ticket ticket);
        Task Add(TicketDTO ticketDto, int userId);
        Task pay(int [] ids);
        Task Delete(int id);
    }
}
