using server.Models;
using server.Models.DTO;

namespace server.BLL.Intefaces
{
    public interface IGiftService
    {
        Task<IEnumerable<GiftDtoResult>> Get();
        Task<GiftDtoResult> Get(int id);
        Task Add(Gift gift);
        Task Update(int id, GiftDto gift);
        Task<bool> Delete(int id);
        Task<IEnumerable<GiftDtoResult>> Search(string giftName = null, string donorName = null, int? buyerCount = null);
        Task<DonorDtoResult> GetDonor(int giftId);
        public Task<bool> TitleExists(string title);
        public Task<IEnumerable<GiftDtoResult>> SortByPrice();
        public Task<IEnumerable<GiftDtoResult>> SortByCategory();
        public Task raffle(int id);
    }
}
