using backend.Data.Models;
using backend.Data.Repositories;
using backend.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Api.Controllers;

[Route("api/category/")]
[ApiController]
public class CategoryController : ControllerBase
{
    
    private readonly IProductRepository _productRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly ILogger<ProductRepository> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public CategoryController(
        ICategoryRepository categoryRepo,
        IProductRepository productRepo,
        ILogger<ProductRepository> logger,
        IHttpClientFactory httpClientFactory
    )
    {
        _categoryRepo = categoryRepo;
        _productRepo = productRepo;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        try
        {
            var categories = await _categoryRepo.GetAllCategories();
            return Ok(categories);
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

    [HttpPost]
    public async Task<IActionResult> CreateCategory(Category category)
    {
        try
        {
            await _categoryRepo.CreateCategory(category);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpDelete] public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            await _categoryRepo.DeleteCategory(id);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}