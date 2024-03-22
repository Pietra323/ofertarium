using backend.Data.Models;
using backend.Data.Repositories;
using backend.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        
        public ProductController(
            IProductRepository productRepo,
            ILogger<ProductRepository> logger
            )
        {
            _productRepo = productRepo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(int UserId, Product product)
        {
            try
            {
                var createdUser = await _productRepo.CreateProduct(UserId,product);
                return CreatedAtAction(nameof(AddProduct), createdUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
