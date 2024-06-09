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
using backend.Data.Models.DataBase;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ILogger<UserController> _logger;
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IPaymentRepository _cardRepo;

        public UserController(
            IUserRepository userRepo,
            ILogger<UserController> logger,
            IProductRepository productRepo,
            ICategoryRepository categoryRepo,
            IPaymentRepository cardRepo
        )
        {
            _userRepo = userRepo;
            _logger = logger;
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _cardRepo = cardRepo;
        }

        
[HttpPost("seed")]
[SwaggerOperation(Summary = "Seedowanie użytkowników")]
public async Task<IActionResult> SeedUsers(int count)
{
    try
    {
        var categories = new[] {
            "Elektronika",
            "Moda",
            "Dom i Ogród",
            "Dziecko",
            "Kultura i Rozrywka",
            "Zdrowie",
            "Sport i Turystyka",
            "Uroda",
            "Motoryzacja",
            "Kolekcje i Sztuka"
        };

        // Add categories only if they don't exist
        foreach (var categoryString in categories)
        {
            var existingCategory = await _categoryRepo.GetCategoryByName(categoryString);
            if (existingCategory == null)
            {
                var category = new Category
                {
                    Nazwa = categoryString,
                    Description = "..."
                };
                await _categoryRepo.CreateCategory(category);
            }
        }

        var samplePhotos = new List<string>
        {
            "photo1.jpg",
            "photo2.jpg",
            "photo3.jpg"
        };

        var existingUsersCount = await _userRepo.GetUserCount();
        var usersToAdd = count - existingUsersCount;

        for (int i = existingUsersCount + 1; i <= usersToAdd + existingUsersCount; i++)
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

            Random random = new Random();
            int min = 1;
            int max = 10;
            int randomNumberInRange = random.Next(min, max);
            var product = new ProductDTO()
            {
                ProductName = $"ProductName{i}",
                Subtitle = $"Subtitle of the product {i}",
                amountOf = i % 5,
                Price = randomNumberInRange,
                CategoryIds = new List<int> { (i + 7) % 11, (i + 4) % 11, (i + 1) % 11 },
                Photos = samplePhotos
            };
            string cardNumberString = string.Empty;
            for (int j = 0; j < 16; j++)
            {
                cardNumberString += random.Next(0, 10).ToString();
            }

            await _userRepo.CreatePersonAsync(user);

            long cardNumber = long.Parse(cardNumberString);
            for (int k = 0; k <= 1; k++)
            {
                randomNumberInRange = random.Next(min, max);
                var paymentCard = new PaymentCard()
                {
                    OwnerFName = user.Name,
                    OwnerLName = user.LastName,
                    OwnerNickname = user.Username,
                    CardNumber = cardNumber,
                    UserId = user.Id,
                    Balance = Math.Round((decimal)randomNumberInRange * 100, 2)
                };
                await _cardRepo.CreatePaymentCard(paymentCard);
            }

            var productDT = await _productRepo.CreateProduct(user.Id, product);
            await _categoryRepo.AddCategoryProducts(productDT.IdProduct, product.CategoryIds);
        }

        return Ok("Użytkownicy zostali dodani.");
    }
    catch (Exception e)
    {
        _logger.LogError(e.Message);
        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
    }
}


        
        [HttpGet("authstatus")]
        [SwaggerOperation(Summary = "Pobierz swój status")]
        public IActionResult AuthStatus()
        {
            if (User.Identity.IsAuthenticated)
            {
                var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
                return Ok(new { isAuthenticated = true, userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value, roles });
            }
            return Ok(new { isAuthenticated = false });
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
        [SwaggerOperation(Summary = "Zaaktualizuj użytkownika")]
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
                existingUser.LastName = userToUpdate.LastName;
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
        [SwaggerOperation(Summary = "Usuń dowolnego użytkownika !DLA ADMINA!")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var existingUser = await _userRepo.GetPeopleByIdAsync(id);
                var deletedPerson = _userRepo.DeletePersonAsync(existingUser);
                return Ok(deletedPerson);
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

        [HttpDelete]
        [Authorize]
        [SwaggerOperation(Summary = "Usuń swoje konto")]
        public async Task<IActionResult> DeleteMyself()
        {
            int? userId = Auth.GetUserId(HttpContext);
            if (userId == null)
            {
                return Unauthorized();
            }
            var user = await _userRepo.GetPeopleByIdAsync(userId.Value);
            await _userRepo.DeletePersonAsync(user);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Supcio");
        }



        [HttpGet]
        [SwaggerOperation(Summary = "Pobierz wszystkich użytkowników")]
        [Authorize]
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

        [HttpGet("currentUser")]
        [SwaggerOperation(Summary = "Pobierz użytkownika")]
        public async Task<IActionResult> GetUserById()
        {
            try
            {
                int? userId = Auth.GetUserId(HttpContext);
                if (userId == null)
                {
                    Unauthorized();
                }
                var user = await _userRepo.GetPeopleByIdAsync(userId.Value);
                if (user == null)
                {
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = "record not found"
                    });
                }

                var userDto = new UserDTO()
                {
                    Id = user.Id,
                    Name = user.Name,
                    LastName = user.LastName,
                    Username = user.Username,
                    Email = user.Email,
                    isAdmin = user.isAdmin
                };

                return Ok(userDto);
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
                        new Claim(ClaimTypes.Role, user.isAdmin ? "Administrator" : "Użytkownik")
                    };

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
        [SwaggerOperation(Summary = "Wylogowanie użytkownika")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logged out successfully." });
        }
    }
}
