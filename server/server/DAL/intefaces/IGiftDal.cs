using server.Models;
using server.Models.DTO;

namespace server.DAL.intefaces
{
    public interface IGiftDal
    {
        Task<List<GiftDtoResult>> Get();
        Task<GiftDtoResult> Get(int id);
        Task Add(Gift gift);
        Task Update(int id,GiftDto gift);
        Task<bool> Delete(int id);
        Task<List<GiftDtoResult>> Search(string giftName = null, string donorName = null, int? buyerCount = null);
        Task<DonorDtoResult> GetDonor(int giftId);
        public Task<bool> TitleExists(string title);
        public Task<List<GiftDtoResult>> SortByPrice();
        public Task<List<GiftDtoResult>> SortByCategory();
        public Task UpdateWinnerId(int id, int winnerId);
    }
}