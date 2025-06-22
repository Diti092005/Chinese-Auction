using server.Models;
using server.Models.DTO;

namespace server.DAL.intefaces
{
    public interface IDonorDal
    {
        public Task<List<DonorDtoResult>> Get();
        public Task<DonorDtoResult> Get(int id);
        public Task Add(Donor donor);
        public Task Update(int id, DonorDto donorDto);
        public Task Delete(int id);
        public Task<List<DonorDtoResult>> Search(string name = null, string email = null, string giftName = null);
    }
}
