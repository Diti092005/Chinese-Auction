using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using server.Bll.Interfaces;
using server.Models;
using server.Models.DTO;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        public IMapper Mapper { get; }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            _logger.LogInformation($"User login attempt: {loginDto.Username}");
            try
            {
                var token = await _userService.Login(loginDto.Username, loginDto.Password);
                _logger.LogInformation($"User {loginDto.Username} logged in successfully");
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, $"Unauthorized login attempt for user {loginDto.Username}");
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error in Login for user {loginDto.Username}");
                return StatusCode(500, "An unexpected error occurred: ");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            _logger.LogInformation($"User registration attempt: {registerDto.Username}");
            try
            {
                var user = _mapper.Map<User>(registerDto);

                await _userService.Register(user);
                _logger.LogInformation($"User {registerDto.Username} registered successfully");
                return Ok("User registered successfully.");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"Registration failed for user {registerDto.Username}");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error in Register for user {registerDto.Username}");
                return StatusCode(500, "An unexpected error occurred: ");
            }
        }
        [HttpGet("{username}")]
        public async Task<IActionResult> UsernameExist(string username)
        {
            try
            {
                return Ok(await _userService.UsernameExist(username));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: ");
            }

        }
    }
}
