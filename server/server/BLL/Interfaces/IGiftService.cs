using server.Models;
using server.Models.DTO;

namespace server.Bll.Interfaces
{
    public interface IGiftService
    {
        Task<IEnumerable<Gift>> Get();
        Task<Gift> Get(int id);
        Task Add(Gift gift);
        Task Update(int id, GiftDTO gift);
        Task<bool> Delete(int id);
        Task<IEnumerable<Gift>> Search(string giftName = null, string donorName = null, int? buyerCount = null);
        Task<Donor> GetDonor(int giftId);
        public Task<bool> TitleExists(string title);
        public Task<IEnumerable<Gift>> SortByPrice();
        public Task<IEnumerable<Gift>> SortByCategory();
        public Task raffle(int id);
    }
}
