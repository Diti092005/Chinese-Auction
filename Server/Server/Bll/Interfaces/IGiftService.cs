using Server.Models;
using Server.Models.DTO;

namespace Server.Bll.Interfaces
{
    public interface IGiftService
    {
        Task<IEnumerable<Gift>> Get();
        Task<Gift> Get(int id);
        Task Add(GiftDTO giftDto);
        Task Update(int id, GiftDTO giftDto);
        Task<bool> Delete(int id);
        Task<IEnumerable<Gift>> Search(string? giftName = null, string? donorName = null, int? buyerCount = null);
        Task<Donor> GetDonor(int giftId);
        public Task<bool> TitleExists(string title);
        public Task<IEnumerable<Gift>> SortByPrice();
        public Task<IEnumerable<Gift>> SortByCategory();
        public Task raffle(int id);
    }
}
