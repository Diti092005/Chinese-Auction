using server.BLL.Intefaces;
using server.DAL;
using server.DAL.intefaces;
using server.Models;
using server.Models.DTO;

namespace server.BLL
{
    public class GiftService : IGiftService
    {
        private readonly IGiftDal _giftDal;
        private readonly ITicketDal _ticketDal;

        public GiftService(IGiftDal giftRepository, ITicketDal ticketDal)
        {
            _giftDal = giftRepository;
            _ticketDal = ticketDal;
        }
        public async Task<IEnumerable<GiftDtoResult>> Get()
        {
            return await _giftDal.Get();
        }
        public async Task<GiftDtoResult> Get(int id)
        {
            return await _giftDal.Get(id);
        }
        public async Task Add(Gift gift)
        {
            await _giftDal.Add(gift);
        }
        public async Task Update(int id, GiftDto gift)
        {
            await _giftDal.Update(id, gift);
        }
        public async Task<bool> Delete(int id)
        {
            return await _giftDal.Delete(id);
        }
        public async Task<IEnumerable<GiftDtoResult>> Search(string giftName = null, string donorName = null, int? buyerCount = null)
        {
            return await _giftDal.Search(giftName, donorName, buyerCount);
        }
        public async Task<DonorDtoResult> GetDonor(int giftId)
        {
            return await _giftDal.GetDonor(giftId);
        }
        public async Task<bool> TitleExists(string title)
        {
            return await _giftDal.TitleExists(title);
        }
        public async Task<IEnumerable<GiftDtoResult>> SortByPrice()
        {
            return await _giftDal.SortByPrice();
        }
        public async Task<IEnumerable<GiftDtoResult>> SortByCategory()
        {
            return await _giftDal.SortByCategory();
        }

        public async Task raffle(int id)
        {
            var gift = await _giftDal.Get(id);
            if (gift == null)
            {
                throw new InvalidOperationException("Gift not found");
            }
            if (gift.Winner != null)
            {
                throw new InvalidOperationException("Gift already won");
            }
            var numOfTickets = gift.Tickets.Count;
            if (numOfTickets == 0)
            {
                throw new InvalidOperationException("No tickets sold");
            }
            var random = new Random();
            var winnerIndex = random.Next(0, numOfTickets);
            var winnerTicket = gift.Tickets[winnerIndex];

            await _ticketDal.Win(winnerTicket.Id);
            await _giftDal.UpdateWinnerId(id, winnerTicket.User.Id);
        }

    }
}
