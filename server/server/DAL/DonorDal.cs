using AutoMapper;
using Microsoft.EntityFrameworkCore;
using server.DAL.intefaces;
using server.Models;
using server.Models.DTO;

namespace server.DAL
{
    public class DonorDal : IDonorDal
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DonorDal(AppDbContext context, IMapper mapper)
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

        public async Task<List<DonorDtoResult>> Get()
        {
            var donors = await _context.Donors
                .Include(d => d.Gifts)
                .ToListAsync();
            if (donors == null || !donors.Any())
            {
                throw new InvalidOperationException("No donors found.");
            }

            var donorDtos = _mapper.Map<List<DonorDtoResult>>(donors);
            return donorDtos;
        }

        public async Task<DonorDtoResult> Get(int id)
        {
            var donor = await _context.Donors
                .Include(d => d.Gifts)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donor == null)
            {
                throw new KeyNotFoundException($"Donor with ID {id} not found.");
            }

            var donorDto = _mapper.Map<DonorDtoResult>(donor);
            return donorDto;
        }

        public async Task Update(int id, DonorDto donorDto)
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

        public async Task<List<DonorDtoResult>> Search(string name = null, string email = null, string giftName = null)
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
                throw new InvalidOperationException("No donors match the search criteria.");
            }
            var donorDtos = _mapper.Map<List<DonorDtoResult>>(donors);
            return donorDtos;
        }
    }
}
