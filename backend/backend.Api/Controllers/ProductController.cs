using System.Security.Claims;
using backend.Data.Models;
using backend.Data.Repositories;
using System.Collections.Generic;
using backend.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
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
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(
            IProductRepository productRepo,
            ILogger<ProductRepository> logger,
            IHttpClientFactory httpClientFactory
        )
        {
            _productRepo = productRepo;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Dodaj produkt")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            int? userId = Auth.GetUserId(HttpContext);
            if (userId == null)
            {
                Unauthorized();
            }

            try
            {
                var createdProduct = await _productRepo.CreateProduct(userId.Value, product);
                return Ok($"Produkt został dodany pomyślnie dla użytkownika o Id = {userId}.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "Wystąpił błąd podczas dodawania produktu.");
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
        
        
        [HttpGet("{category}/products")]
        [SwaggerOperation(Summary = "Pobierz produkty po kategorii")]
        public async Task<IActionResult> GetAllProductsByCategory(int category)
        {
            try
            {
                var products = await _productRepo.GetAllProductsByCategory(category);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Pobierz produkt po Id")]
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
            Console.WriteLine(e);
            throw;
        }
    }
    /*

    [HttpPost]
    public async Task<IActionResult> c(int UserId, Product product)
    {
        return null;
    }
    */
    }
}
