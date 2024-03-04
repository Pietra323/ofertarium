using backend.Data.Models;
using backend.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

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

    }
}
