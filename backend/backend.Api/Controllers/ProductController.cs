using backend.Data.Models;
using backend.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Api.Controllers;

[Route("api/product/")]
[ApiController]

public class ProductController : Controller
{
    private readonly IProductRepository _productRepo;
    private readonly ILogger<ProductController> _logger;
    
    public ProductController(
        IProductRepository prodRepo,
        ILogger<ProductController> logger
    )
    {
        _productRepo = prodRepo;
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddProduct(int userId, Product product)
    {
        try
        {
            var createdProduct = await _productRepo.CreateProduct(userId, product);
            return CreatedAtAction(nameof(AddProduct), createdProduct);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                e.Message);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetProducts(int userId)
    {
        try
        {
            var products = await _productRepo.GetAllProducts(userId);
            return Ok(products);
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