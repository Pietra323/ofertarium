using backend.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using backend.Data.Models;
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

        
        [HttpPost("seed")]
        [SwaggerOperation(Summary = "Seedowanie użytkowników")]
        public async Task<IActionResult> SeedUsers(int count)
        {
            try
            {
                for (int i = 1; i <= count; i++)
                {
                    var user = new User
                    {
                        Name = $"FirstName{i}",
                        LastName = $"LastName{i}",
                        Username = $"user{i}",
                        Email = $"user{i}@example.com",
                        Password = $"password{i}{i}{i}.",
                        isAdmin = false
                    };

                    await _userRepo.CreatePersonAsync(user);
                }

                return Ok("Użytkownicy zostali dodani.");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
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
        [Authorize]
        public async Task<IActionResult> UpdateUser(User userToUpdate)
        {
            try
            {
                int? userId = Auth.GetUserId(HttpContext);
                if (userId == null)
                {
                    Unauthorized();
                }
                var existingUser = await _userRepo.GetPeopleByIdAsync(userId.Value);
                if (existingUser == null)
                {
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = "record not found"
                    });
                }

                existingUser.Name = userToUpdate.Name;
                existingUser.Password = userToUpdate.Password;
                existingUser.Username = userToUpdate.Username;
                existingUser.Email = userToUpdate.Email;
                await _userRepo.UpdatePersonAsync(existingUser);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        statusCode = 500,
                        message = e.Message
                    });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var existingUser = await _userRepo.GetPeopleByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = "record not found"
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
                        statusCode = 500,
                        message = e.Message
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
                        statusCode = 500,
                        message = e.Message
                    });
            }
        }

        [HttpGet("Id")]
        [Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userRepo.GetPeopleByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = "record not found"
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
                        statusCode = 500,
                        message = e.Message
                    });
            }
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Logowanie użytkownika")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginModel)
        {
            try
            {
                var user = await _userRepo.LoginUser(loginModel.Username, loginModel.Password);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, "Użytkownik")
                    };

                    if (user.isAdmin)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
                    }

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return Ok(new { message = "Logged in successfully." });
                }
                else
                {
                    return Unauthorized(new { message = "Invalid username or password." });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred during login");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }


        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logged out successfully." });
        }
    }
}
