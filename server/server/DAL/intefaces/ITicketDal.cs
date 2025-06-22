using server.Models;
using server.Models.DTO;

namespace server.DAL.intefaces
{
    public interface ITicketDal
    {
        Task<List<TicketDtoResult>> Get();
        Task<List<TicketDtoResult>> GetByUserPaid();
        Task<List<TicketDtoResult>> GetByUserPending();
        Task<TicketDtoResult> Get(int id);
        Task<List<TicketDtoResult>> GetByGiftId(int giftId);

        Task Add(Ticket ticket);
        Task pay(int id);
        Task Win(int id);
        Task Delete(int id);

    }
}
