using backend.Data.Repositories;
using backend.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ILogger<BasketRepository> _logger;

        public BasketController(
            IBasketRepository basketRepository, 
            ILogger<BasketRepository> logger
            )
        {
            _basketRepository = basketRepository;
            _logger = logger;
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
        
        [HttpDelete("delete")]
        public async Task<IActionResult> RemoveFromBasket(int productId)
        {
            try
            {
                int? userId = Auth.GetUserId(HttpContext);
                if (userId == null)
                {
                    Unauthorized();
                }
                var basket = await _basketRepository.RemoveFromBasket(userId.Value, productId);
                return Ok(basket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd podczas dodawania produktu do koszyka: {ex.Message}");
            }
        }

        [HttpGet("summary")]
        public async Task<IActionResult> SummaryBasket()
        {
            try
            {
                int? userId = Auth.GetUserId(HttpContext);
                if (userId == null)
                {
                    return Unauthorized();
                }

                var basketProducts = await _basketRepository.SummaryBasket(userId.Value);
                if (basketProducts == null)
                {
                    return NotFound("Koszyk nie został znaleziony.");
                }

                return Ok(basketProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error summarizing basket");
                return StatusCode(500, $"Wystąpił błąd podczas podsumowania koszyka: {ex.Message}");
            }
        }

    }
}
