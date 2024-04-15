using backend.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToBasket(int productId)
        {
            try
            {
                int? userId = Auth.GetUserId(HttpContext);
                if (userId == null)
                {
                    Unauthorized();
                }
                await _basketRepository.AddToBasket(userId.Value, productId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd podczas dodawania produktu do koszyka: {ex.Message}");
            }
        }
    }
}
