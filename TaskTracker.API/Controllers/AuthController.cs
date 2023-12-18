using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskTracker.API.DTO;
using TaskTracker.DataAccess.Repository.Contracts;
using TaskTracker.Domain.Entities;

namespace TaskTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register(UserRegistrationDTO userDto)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            if (userDto == null)
                return NotFound();

            if ( string.IsNullOrEmpty(userDto.FirstName) ||
                 string.IsNullOrEmpty(userDto.LastName) ||
                 string.IsNullOrEmpty(userDto.Username) ||
                 string.IsNullOrEmpty(userDto.Password) )
            {
                return BadRequest("All fields need to be populated.");
            }                

            try
            {
                var user = new User
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Username = userDto.Username,
                    PasswordHash = passwordHash
                };
                _unitOfWork.UserRepository.Add(user);
                _unitOfWork.Save();
            }
            catch(DbUpdateException ex)
            {
                return StatusCode(500, new { error = "Internal server error.", ex.Message });
            }

            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDTO userDto)
        {
            if (userDto == null)
                return NotFound();

            if (string.IsNullOrEmpty(userDto.Username) && string.IsNullOrEmpty(userDto.Password))
                return BadRequest("You have to provide username and password.");

            var user = _unitOfWork.UserRepository.GetExp(x => x.Username == userDto.Username);

            if (user == null)
                return NotFound();
            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
                return BadRequest("Username or password is wrong.");

            string jwtToken = CreateJwtToken(user);

            return Ok(jwtToken);
        }

        private string CreateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Token").Value!));

            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddHours(4),
                    signingCredentials: credentials
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
