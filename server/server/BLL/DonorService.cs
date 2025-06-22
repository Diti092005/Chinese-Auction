using server.BLL.Intefaces;
using server.DAL.intefaces;
using server.Models;
using server.Models.DTO;

namespace server.BLL
{
    public class DonorService:IDonorService
    {
        private readonly IDonorDal _donorDal;
        public DonorService(IDonorDal donorDal)
        {
            _donorDal = donorDal;
        }
        public async Task<IEnumerable<DonorDtoResult>> Get()
        {
            return await _donorDal.Get();
        }
        public async Task<DonorDtoResult> Get(int id)
        {
            return await _donorDal.Get(id);
        }
        public async Task Add(Donor donor)
        {
            await _donorDal.Add(donor);
        }
        public async Task Update(int id, DonorDto donorDto)
        {
            await _donorDal.Update(id, donorDto);
        }
        public async Task Delete(int id)
        {
            await _donorDal.Delete(id);
        }
        public async Task<IEnumerable<DonorDtoResult>> Search(string name = null, string email = null, string giftName = null)
        {
            return await _donorDal.Search(name, email, giftName);
        }
    }
}
