using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Data.Models;
using backend.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ILogger<UserController> _logger;
        
        public UserController(
            IUserRepository userRepo,
            ILogger<UserController> logger
            )
        {
            _userRepo = userRepo;
            _logger = logger;
        }
        
        [HttpPost]
        [SwaggerOperation(Summary = "Dodaj nowego użytkownika")]
        public async Task<IActionResult> AddUser(User user)
        {
            try
            {
                var createdUser = await _userRepo.CreatePersonAsync(user);
                return CreatedAtAction(nameof(AddUser), createdUser);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateUser(User userToUpdate)
        {
            try
            {
                var existingUser = await _userRepo.GetPeopleByIdAsync (userToUpdate.Id);
                if (existingUser.Equals(null))
                {
                    return NotFound(new
                    {
                        statusCode=404, message="record not found"
                    });
                }

                existingUser.Name = userToUpdate.Name;
                await _userRepo.UpdatePersonAsync(existingUser);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        statusCode=500, message=e.Message
                    });
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var existingUser = await _userRepo.GetPeopleByIdAsync (id);
                if (existingUser.Equals(null))
                {
                    return NotFound(new
                    {
                        statusCode=404, message="record not found"
                    });
                }

                await _userRepo.DeletePersonAsync(existingUser);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        statusCode=500, message=e.Message
                    });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userRepo.GetPeopleAsync();
                return Ok(users);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        statusCode=500, message=e.Message
                    });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userRepo.GetPeopleByIdAsync(id);
                if (user.Equals(null))
                {
                    return NotFound(new
                    {
                        statusCode=404, message="record not found"
                    });
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        statusCode=500, message=e.Message
                    });
            }
        }

        
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var user = await _userRepo.LoginUser(username, password);
                if (user != null)
                {
                    var token = GenerateJwtToken(user);
                    return Ok(new { Token = token });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("a5ff671c7b8b5dbe3ed056bbafe8edae9fe86e53845b6d32707ee96e99feb5bb"); // Sekretny klucz do podpisywania tokena
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    // Tutaj możesz dodać więcej roszczeń (claims) w zależności od potrzeb
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Czas wygaśnięcia tokena (np. 1 godzina)
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
