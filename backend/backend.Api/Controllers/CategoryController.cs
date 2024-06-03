using backend.Data.Models;
using backend.Data.Repositories;
using backend.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Api.Controllers;

[Route("api/category/")]
[ApiController]
public class CategoryController : ControllerBase
{
    
    private readonly IProductRepository _productRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly ILogger<CategoryController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public CategoryController(
        ICategoryRepository categoryRepo,
        IProductRepository productRepo,
        ILogger<CategoryController> logger,
        IHttpClientFactory httpClientFactory
    )
    {
        _categoryRepo = categoryRepo;
        _productRepo = productRepo;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }
    
    [HttpGet]
    [Authorize(Roles = "Administrator")]
    [SwaggerOperation(Summary = "Pobierz wszystkie kategorie")]
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
    [Authorize(Roles = "Administrator")]
    [SwaggerOperation(Summary = "Stwórz kategorię")]
    public async Task<IActionResult> CreateCategory(Category kategoria)
    {
        try
        {
            await _categoryRepo.CreateCategory(kategoria);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [Authorize(Roles = "Administrator")]
    [SwaggerOperation(Summary = "Usuń kategorię")]
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