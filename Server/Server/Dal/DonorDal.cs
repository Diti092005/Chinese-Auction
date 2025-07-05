using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Dal.Interfaces;
using Server.Models;
using Server.Models.DTO;
using Server.Dal;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
namespace Server.Dal
{
    public class DonorDal : IDonorDal
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<DonorDal> _logger;

        public DonorDal(ApplicationDbContext context, IMapper mapper, ILogger<DonorDal> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task Add(Donor donor)
        {
            _logger.LogInformation($"Adding donor: {donor?.Name}");
            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Donor added successfully");
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation($"Deleting donor with id {id}");
            var donor = await _context.Donors.Include(d => d.Gifts).FirstOrDefaultAsync(d => d.Id == id);
            if (donor == null)
            {
                _logger.LogWarning($"Donor with ID {id} not found");
                throw new KeyNotFoundException($"Donor with ID {id} not found.");
            }
            if (donor.Gifts != null && donor.Gifts.Any())
            {
                _logger.LogWarning($"Cannot delete donor with ID {id} because he has gifts.");
                // Custom exception for client-friendly message
                throw new InvalidOperationException("Cannot delete donor because he has gifts. Please remove all gifts before deleting the donor.");
            }
            _context.Donors.Remove(donor);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Donor with id {id} deleted successfully");
        }

        public async Task<List<Donor>> Get()
        {
            _logger.LogInformation("Getting all donors");
            var donors = await _context.Donors
                .Include(d => d.Gifts)
                .ToListAsync();
            if (donors == null || !donors.Any())
            {
                _logger.LogWarning("No donors found");
                throw new InvalidOperationException("No donors found.");
            }
            _logger.LogInformation($"Returned {donors.Count} donors");
            return donors;
        }

        public async Task<Donor> Get(int id)
        {
            _logger.LogInformation($"Getting donor with id {id}");
            var donor = await _context.Donors
                .Include(d => d.Gifts)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (donor == null)
            {
                _logger.LogWarning($"Donor with ID {id} not found");
                throw new KeyNotFoundException($"Donor with ID {id} not found.");
            }
            _logger.LogInformation($"Returned donor with id {id}");
            return donor;
        }

        public async Task Update(int id, DonorDTO donorDto)
        {
            _logger.LogInformation($"Updating donor with id {id}");
            var existingDonor = await _context.Donors.FindAsync(id);
            if (existingDonor == null)
            {
                _logger.LogWarning($"Donor with ID {id} not found");
                throw new KeyNotFoundException($"Donor with ID {id} not found.");
            }
            if (existingDonor != null)
            {
                existingDonor.Name = donorDto.Name;
                existingDonor.Email = donorDto.Email;
                existingDonor.ShowMe = donorDto.ShowMe;

                _context.Donors.Update(existingDonor);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Donor with id {id} updated successfully");
            }
        }

        public async Task<List<Donor>> Search(string? name = null, string? email = null, string? giftName = null)
        {
            _logger.LogInformation("Searching donors");
            var query = _context.Donors
                .Include(d => d.Gifts)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(d => d.Name.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(d => d.Email.Contains(email));
            }

            if (!string.IsNullOrWhiteSpace(giftName))
            {
                query = query.Where(d => d.Gifts.Any(g => g.GiftName.Contains(giftName)));
            }
            var donors = await query.ToListAsync();
            if (donors == null || !donors.Any())
            {
                _logger.LogWarning("No donors match the search criteria");
                throw new InvalidOperationException("No donors match the search criteria.");
            }
            _logger.LogInformation($"Returned {donors.Count} matching donors");
            return donors;
        }
    }
}
