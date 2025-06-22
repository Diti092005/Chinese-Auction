using Microsoft.EntityFrameworkCore;
using server.DAL.intefaces;
using server.Models;
using System.Security.Claims;

namespace server.DAL
{
    public class UserDal : IUserDal
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserDal(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserFromToken()
        {
            var usernameFromToken = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(usernameFromToken))
            {
                throw new UnauthorizedAccessException("Username not found in token.");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == usernameFromToken);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }
            return user;
        }

        public async Task<bool> UsernameExist(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null)
                return true;
            return false;
        }
    }
}
