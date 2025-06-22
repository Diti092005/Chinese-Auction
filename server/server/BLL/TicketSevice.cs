using server.BLL.Intefaces;
using server.DAL.intefaces;
using server.Models;
using server.Models.DTO;

namespace server.BLL
{
    public class TicketSevice : ITicketService
    {
        private readonly ITicketDal _ticketDal;
        private readonly IGiftDal _giftDal;

        public TicketSevice(ITicketDal ticketDal, IGiftDal giftDal)
        {
            _ticketDal = ticketDal;
            _giftDal = giftDal;
        }
        public async Task<IEnumerable<TicketDtoResult>> Get()
        {
            return await _ticketDal.Get();
        }

        public async Task<IEnumerable<TicketDtoResult>> GetByUserPaid()
        {
            return await _ticketDal.GetByUserPaid();
        }

        public async Task<IEnumerable<TicketDtoResult>> GetByUserPending()
        {
            return await _ticketDal.GetByUserPending();
        }

        public async Task<TicketDtoResult> Get(int id)
        {
            return await _ticketDal.Get(id);
        }

        public async Task Add(Ticket ticket)
        {
            await _ticketDal.Add(ticket);
        }

        public async Task pay(int id)
        {
            await _ticketDal.pay(id);
        }

        public async Task Delete(int id)
        {
            await _ticketDal.Delete(id);
        }

        public async Task<IEnumerable<TicketDtoResult>> GetByGiftId(int giftId)
        {
            return await _ticketDal.GetByGiftId(giftId);
        }


    }
}
