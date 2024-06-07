using System.Security.Claims;
using backend.Data.Models;
using backend.Data.Repositories;
using System.Collections.Generic;
using backend.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using backend.Data.Models.ManyToManyConnections;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Api.Controllers
{
    [Route("api/products/")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        private readonly ILogger<ProductRepository> _logger;
        private readonly IUserRepository _userRepo;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(
            IProductRepository productRepo,
            ILogger<ProductRepository> logger,
            IHttpClientFactory httpClientFactory,
            IUserRepository userRepo
        )
        {
            _userRepo = userRepo;
            _productRepo = productRepo;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [Authorize]
        [HttpPost("add_product")]
        [SwaggerOperation(Summary = "Dodaj produkt")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            try
            {
                // Sprawdź czy UserId nie jest null przed dostępem
                if (product.UserId == null)
                {
                    return BadRequest("UserId cannot be null.");
                }

                // Dodanie logowania dla sprawdzenia wartości ExpirationTime
                _logger.LogInformation($"Adding product without expiration time: {product.ProductName}");

                // Dodaj produkt do repozytorium
                await _productRepo.CreateProduct(product.UserId.Value, product);

                return Ok("Product added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [Authorize]
        [SwaggerOperation(Summary = "Dodaj promocję czasową")]
        [HttpPost("add_on_sale")]
        public async Task<IActionResult> AddOnSale(int days, int months, int hours, int minutes, decimal newPrice, int productId)
        {
            try
            {
                await _productRepo.AddOnSale(days, months, hours, minutes, newPrice, productId);
                return Ok("Product put on sale successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Usuń produkt")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var existingProduct = await _productRepo.GetProductById(id);
                if (existingProduct == null)
                {
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = "record not found"
                    });
                }

                await _productRepo.DeleteProduct(existingProduct);
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
        [SwaggerOperation(Summary = "Pobierz wszystkie produkty")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productRepo.GetAllProducts();
                return Ok(products);
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


        [HttpPost("add-favourite")]
        [Authorize]
        [SwaggerOperation(Summary = "Dodaj produkt do ulubionych użytkownika")]
        public async Task<IActionResult> AddFavourite(int productId)
        {
            try
            {
                int? userId = Auth.GetUserId(HttpContext);
                if (userId == null)
                {
                    Unauthorized();
                }

                var product = await _productRepo.GetProductById(productId);
                if (product == null)
                {
                    return NotFound(new { message = "Product not found" });
                }

                var favourite = await _productRepo.GetFavouriteByUserIdAndProductIdAsync(userId.Value, productId);
                if (favourite == null)
                {
                    favourite = new Favourite
                    {
                        Description = $"Favourite product {product.ProductName}"
                    };
                    await _productRepo.CreateFavouriteAsync(favourite);
                }

                var userFavourite = new UserFavourite
                {
                    UserId = userId.Value,
                    ProductId = productId,
                    FavouriteId = favourite.Id
                };

                await _productRepo.AddUserFavouriteAsync(userFavourite);

                return Ok(new { message = "Product added to favourites successfully" });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }
        
        [HttpGet("/user/favourite")]
        [Authorize]
        [SwaggerOperation(Summary = "Pobierz ulubione produkty użytkownika")]
        public async Task<IActionResult> GetUserFavourites()
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
                    return NotFound(new { message = "User not found" });
                }

                var userFavourites = await _productRepo.GetUserFavouritesByUserIdAsyncNOW(userId.Value);
                return Ok(userFavourites);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }
        
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Pobierz produkt")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productRepo.GetProductById(id);
                if (product == null)
                {
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = "record not found"
                    });
                }
                return Ok(product);
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

        [HttpGet("{id}/onsale")]
        [SwaggerOperation(Summary = "Pobierz informacje OnSale danego produktu")]
        public async Task<IActionResult> GetProductOnSale(int id)
        {
            try
            {
                var onSale = await _productRepo.GetOnSaleByProductId(id);
                if (onSale == null)
                {
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = "OnSale record not found"
                    });
                }
                return Ok(onSale);
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
    }
}
