using server.Models;
using server.Models.DTO;

namespace server.Dal.Interfaces
{
    public interface IDonorDal
    {
        public Task<List<Donor>> Get();
        public Task<Donor> Get(int id);
        public Task Add(Donor donor);
        public Task Update(int id, DonorDTO donorDto);
        public Task Delete(int id);
        public Task<List<Donor>> Search(string name = null, string email = null, string giftName = null);
        public Task<int> CountOfGifts(int donorId); // Added method

    }
}
