using server.Models;
using server.Models.DTO;

namespace server.BLL.Intefaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDtoResult>> Get();
        Task<IEnumerable<TicketDtoResult>> GetByUserPaid();
        Task<IEnumerable<TicketDtoResult>> GetByUserPending();
        Task<IEnumerable<TicketDtoResult>> GetByGiftId(int giftId);

        Task<TicketDtoResult> Get(int id);
        Task Add(Ticket ticket);
        Task pay(int id);
        Task Delete(int id);
    }
}
