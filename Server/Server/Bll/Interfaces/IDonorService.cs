using Server.Dal.Interfaces;
using Server.Models.DTO;
using Server.Models;

namespace Server.Bll.Interfaces
{
    public interface IDonorService
    {
        public Task<IEnumerable<Donor>> Get();
        public Task<Donor> Get(int id);
        public Task Add(DonorDTO donorDto);
        public Task Update(int id, DonorDTO donorDto);
        public Task Delete(int id);
        public Task<IEnumerable<Donor>> Search(string? name = null, string? email = null, string? giftName = null);
    }
}
