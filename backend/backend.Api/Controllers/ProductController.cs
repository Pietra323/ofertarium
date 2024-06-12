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
        
        [HttpGet("api/products/{categoryId}")]
        [SwaggerOperation(Summary = "Pobierz wszystkie produkty z danej kategorii")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _productRepo.GetAllProductsByCategoryAsync(categoryId);
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

        [HttpPost("add_product")]
        [SwaggerOperation(Summary = "Dodaj produkt")]
        public async Task<IActionResult> AddProduct([FromForm] ProductDTO product, [FromForm] List<IFormFile> photos, [FromForm] string description)
        {
            try
            {
                _logger.LogInformation($"Adding product: {product.ProductName}");

                // Dodaj produkt do repozytorium
                var userId = product.UserId;
                var createdProduct = await _productRepo.CreateProduct(userId, product, photos, description);

                return Ok(new { message = "Product added successfully.", product = createdProduct });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }




        [SwaggerOperation(Summary = "Dodaj promocję czasową")]
        [HttpPost("add_on_sale")]
        public async Task<IActionResult> AddOnSale(int days, int months, int hours, int minutes, decimal newPrice, int productId, int userId)
        {
            try
            {

                var product = await _productRepo.GetProductById(productId);
                if (product.UserId != userId)
                {
                    return Ok("Produkt nie należy do użytkownika");
                }
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
        public async Task<IActionResult> DeleteProduct(int id, int userId)
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
                if (existingProduct.UserId != userId)
                {
                    return Ok("Produkt nie należy do użytkownika");
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
        [SwaggerOperation(Summary = "Dodaj produkt do ulubionych użytkownika")]
        public async Task<IActionResult> AddFavourite(int productId, int userId)
        {
            try
            {
                var product = await _productRepo.GetProductById(productId);
                if (product == null)
                {
                    return NotFound(new { message = "Product not found" });
                }

                var favourite = await _productRepo.GetFavouriteByUserIdAndProductIdAsync(userId, productId);
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
                    UserId = userId,
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
        
        [HttpGet("user/products")]
        [SwaggerOperation(Summary = "Pobierz produkty użytkownika")]
        public async Task<IActionResult> ShowUserProducts(int userId)
        {
            try
            {

                var userProducts = await _productRepo.GetAllUserProducts(userId);
                return Ok(userProducts);
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
        
        [HttpDelete("remove-favourite/{productId}")]
        [SwaggerOperation(Summary = "Usuń produkt z ulubionych użytkownika")]
        public async Task<IActionResult> RemoveFavourite(int productId, int userId)
        {
            try
            {

                await _productRepo.RemoveUserFavouriteAsync(userId, productId);

                return Ok(new { message = "Product removed from favourites successfully" });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }
        
        
        [HttpPost("upload-image/{productId}")]
        [SwaggerOperation(Summary = "Upload product image")]
        public async Task<IActionResult> UploadProductImage(int productId, IFormFile file, [FromForm] string description)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Invalid file." });
            }

            try
            {
                await _productRepo.AddPhotoToProduct(productId, file, description);
                return Ok(new { message = "File uploaded successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while uploading the file.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        
        [HttpGet("/user/favourite")]
        [SwaggerOperation(Summary = "Pobierz ulubione produkty użytkownika")]
        public async Task<IActionResult> GetUserFavourites(int userId)
        {
            try
            {
                var user = await _userRepo.GetPeopleByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                var userFavourites = await _productRepo.GetUserFavouritesByUserIdAsyncNOW(userId);
                return Ok(userFavourites);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }
        
        
        [HttpGet("cospobierz")]
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
