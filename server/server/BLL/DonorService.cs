using Microsoft.Extensions.Logging;
using server.Bll.Interfaces;
using server.Dal.Interfaces;
using server.Models;
using server.Models.DTO;

namespace server.Bll
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
            try
            {
                return await _donorDal.Get();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all donors");
                throw;
            }
        }
        public async Task<Donor> Get(int id)
        {
            _logger.LogInformation($"Getting donor with id {id}");
            try
            {
                return await _donorDal.Get(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting donor with id {id}");
                throw;
            }
        }
        public async Task Add(Donor donor)
        {
            _logger.LogInformation($"Adding donor: {donor.Name}");
            try
            {
                await _donorDal.Add(donor);
                _logger.LogInformation($"Donor {donor.Name} added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding donor: {donor.Name}");
                throw;
            }
        }
        public async Task Update(int id, DonorDTO donorDto)
        {
            _logger.LogInformation($"Updating donor with id {id}");
            try
            {
                await _donorDal.Update(id, donorDto);
                _logger.LogInformation($"Donor with id {id} updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating donor with id {id}");
                throw;
            }
        }
        public async Task Delete(int id)
        {
            _logger.LogInformation($"Deleting donor with id {id}");
            try
            {
                await _donorDal.Delete(id);
                _logger.LogInformation($"Donor with id {id} deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting donor with id {id}");
                throw;
            }
        }
        public async Task<IEnumerable<Donor>> Search(string name = null, string email = null, string giftName = null)
        {
            _logger.LogInformation($"Searching donors by name: {name}, email: {email}, giftName: {giftName}");
            try
            {
                return await _donorDal.Search(name, email, giftName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching donors");
                throw;
            }
        }
        public async Task<int> CountOfGifts(int donorId)
        {
            _logger.LogInformation($"Counting gifts for donor id {donorId}");
            try
            {
                return await _donorDal.CountOfGifts(donorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error counting gifts for donor id {donorId}");
                throw;
            }
        }
    }
}
