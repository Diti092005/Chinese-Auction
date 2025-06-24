using server.Models;

namespace server.Dal.Interfaces
{
    public interface ITicketDal
    {
        Task<List<Ticket>> Get();
        Task<List<Ticket>> GetByUserPaid();
        Task<List<Ticket>> GetByUserPending();
        Task<Ticket> Get(int id);
        Task Add(Ticket ticket);
        Task pay(int id);
        Task Win(int id);
        Task Delete(int id);

    }
}
