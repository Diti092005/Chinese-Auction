using Server.Models;

namespace Server.Dal.Interfaces
{
    public interface ITicketDal
    {
        Task<List<Ticket>> Get();
        Task<List<Ticket>> GetByUserPaid();
        Task<List<Ticket>> GetByUserPending();
        Task<List<Ticket>> GetByGiftId(int giftId);

        Task<Ticket> Get(int id);
        Task Add(Ticket ticket);
        Task pay(int [] ids);
        Task Win(int id);
        Task Delete(int id);


    }
}
