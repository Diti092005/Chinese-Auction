using server.Models;
using server.Models.DTO;

namespace server.Bll.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> Get();
        Task<IEnumerable<Ticket>> GetByUserPaid();
        Task<IEnumerable<Ticket>> GetByUserPending();
        Task<Ticket> Get(int id);
        Task Add(Ticket ticket);
        Task pay(int id);
        Task Delete(int id);
    }
}
