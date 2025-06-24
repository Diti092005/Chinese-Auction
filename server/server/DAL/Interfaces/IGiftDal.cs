using server.Models;
using server.Models.DTO;

namespace server.Dal.Interfaces
{
    public interface IGiftDal
    {
        Task<List<Gift>> Get();
        Task<Gift> Get(int id);
        Task Add(Gift gift);
        Task Update(int id,GiftDTO gift);
        Task<bool> Delete(int id);
        Task<List<Gift>> Search(string giftName = null, string donorName = null, int? buyerCount = null);
        Task<Donor> GetDonor(int giftId);
        public Task<bool> TitleExists(string title);
        public Task<List<Gift>> SortByPrice();
        public Task<List<Gift>> SortByCategory();
        public Task UpdateWinnerId(int id, int winnerId);
    }
}