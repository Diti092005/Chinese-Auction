using Server.Bll.Interfaces;
using Server.Dal.Interfaces;
using Server.Models;
using Server.Models.DTO;
using Microsoft.Extensions.Logging;

namespace Server.Bll
{
    public class DonorService:IDonorService
    {
        private readonly IDonorDal _donorDal;
        private readonly ILogger<DonorService> _logger;
        public DonorService(IDonorDal donorDal, ILogger<DonorService> logger)
        {
            _donorDal = donorDal;
            _logger = logger;
        }
        public async Task<IEnumerable<Donor>> Get()
        {
            _logger.LogInformation("Getting all donors");
            var result = await _donorDal.Get();
            _logger.LogInformation($"Returned {result.Count()} donors");
            return result;
        }
        public async Task<Donor> Get(int id)
        {
            _logger.LogInformation($"Getting donor with id {id}");
            var result = await _donorDal.Get(id);
            _logger.LogInformation($"Returned donor with id {id}");
            return result;
        }
        public async Task Add(DonorDTO donorDto)
        {
            if (donorDto == null)
                throw new ArgumentNullException(nameof(donorDto), "Donor data cannot be null.");
            var donor = new Donor
            {
                Name = donorDto.Name,
                Email = donorDto.Email,
                ShowMe = donorDto.ShowMe
            };
            await _donorDal.Add(donor);
        }
        public async Task Update(int id, DonorDTO donorDto)
        {
            if (donorDto == null)
                throw new ArgumentNullException(nameof(donorDto), "Donor data cannot be null.");
            var existingDonor = await _donorDal.Get(id);
            if (existingDonor == null)
                throw new KeyNotFoundException($"Donor with ID {id} not found.");
            await _donorDal.Update(id, donorDto);
        }
        public async Task Delete(int id)
        {
            var existingDonor = await _donorDal.Get(id);
            if (existingDonor == null)
                throw new KeyNotFoundException($"Donor with ID {id} not found.");
            await _donorDal.Delete(id);
        }
        public async Task<IEnumerable<Donor>> Search(string? name = null, string? email = null, string? giftName = null)
        {
            _logger.LogInformation($"Searching donors with name: {name}, email: {email}, giftName: {giftName}");
            var result = await _donorDal.Search(name ?? string.Empty, email ?? string.Empty, giftName ?? string.Empty);
            _logger.LogInformation($"Found {result.Count()} donors");
            return result;
        }
    }
}
