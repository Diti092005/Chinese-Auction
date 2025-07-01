﻿using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Server.Bll.Interfaces;
using Server.Dal.Interfaces;
using Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Server.Bll
{
    public class UserService : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly string _jwtSecret = "1858e87833b36da4ea93df26fb950af27b7a2d1cdddda825eeb443ceeae1fde11cad965006c3c7ef3e927c611e4686981ef08be19a5d38d63b8985542e8893b6"; 
        private readonly ILogger<UserService> _logger;

        public UserService(IUserDal userDal, ILogger<UserService> logger)
        {
            _userDal = userDal;
            _logger = logger;
        }

        public async Task<string> Login(string username, string password)
        {
            _logger.LogInformation($"User login attempt: {username}");
            var user = await _userDal.GetUserByUsername(username);
            if (user == null || !VerifyPassword(password, user.Password))
            {
                _logger.LogWarning($"Invalid login attempt for user: {username}");
                throw new UnauthorizedAccessException("Invalid username or password.");
            }
            _logger.LogInformation($"User {username} logged in successfully");
            return GenerateJwtToken(user);
        }

        public async Task Register(User user)
        {
            _logger.LogInformation($"User registration attempt: {user?.Username}");
            if (user == null)
            {
                _logger.LogWarning("Registration attempt with null user");
                throw new InvalidOperationException("User data cannot be null.");
            }
            var existingUser = await _userDal.GetUserByUsername(user.Username);
            if (existingUser != null)
            {
                _logger.LogWarning($"Registration conflict for user: {user?.Username}");
                throw new InvalidOperationException("Username already exists.");
            }
            user.Password = HashPassword(user.Password);
            await _userDal.AddUser(user);
            _logger.LogInformation($"User {user?.Username} registered successfully");
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim("id", user.Id.ToString()),                  // ← מזהה המשתמש
                    new Claim("fullName", user.FullName ?? "")
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> UsernameExist(string username)
        {
            return await _userDal.UsernameExist(username);
        }
    }
}
