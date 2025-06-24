using AutoMapper;
using Microsoft.EntityFrameworkCore;
using server.Dal.Interfaces;
using server.Models;
using server.Models.DTO;
using server.Dal;
using System.Security.Claims;
namespace server.Dal
{
    public class DonorDal : IDonorDal
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DonorDal(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task Add(Donor donor)
        {
            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var donor = await _context.Donors.FindAsync(id);
            if (donor == null)
            {
                throw new KeyNotFoundException($"Donor with ID {id} not found.");
            }

            _context.Donors.Remove(donor);
            await _context.SaveChangesAsync();

        }

        public async Task<List<Donor>> Get()
        {
            var donors = await _context.Donors
                .Include(d => d.Gifts)
                .ToListAsync();
            if (donors == null || !donors.Any())
            {
                throw new InvalidOperationException("No donors found.");
            }

            return donors;
        }

        public async Task<Donor> Get(int id)
        {
            var donor = await _context.Donors
                .Include(d => d.Gifts)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donor == null)
            {
                throw new KeyNotFoundException($"Donor with ID {id} not found.");
            }

            return donor;
        }

        public async Task Update(int id, DonorDTO donorDto)
        {
            var existingDonor = await _context.Donors.FindAsync(id);
            if (existingDonor == null)
            {
                throw new KeyNotFoundException($"Donor with ID {id} not found.");
            }
            if (existingDonor != null)
            {
                existingDonor.Name = donorDto.Name;
                existingDonor.Email = donorDto.Email;
                existingDonor.ShowMe = donorDto.ShowMe;

                _context.Donors.Update(existingDonor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Donor>> Search(string name = null, string email = null, string giftName = null)
        {
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
                return [];
                //throw new InvalidOperationException("No donors match the search criteria.");
            }
            
            return donors;
        }
        public async Task<int> CountOfGifts(int donorId)
        {
            var donor = await _context.Donors
                .Include(d => d.Gifts)
                .FirstOrDefaultAsync(d => d.Id == donorId);

            if (donor == null)
            {
                throw new KeyNotFoundException($"Donor with ID {donorId} not found.");
            }

            return donor.Gifts?.Count ?? 0;
        }
    }
}
