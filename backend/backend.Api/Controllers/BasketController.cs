using backend.Data.Repositories;
using backend.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        [SwaggerOperation(Summary = "Dodaj produkt do koszyka")]
        public async Task<IActionResult> AddToBasket(int productId, int userId)
        {
            try
            {
                await _basketRepository.AddToBasket(userId, productId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd podczas dodawania produktu do koszyka: {ex.Message}");
            }
        }
        
        [HttpDelete("delete")]
        [SwaggerOperation(Summary = "Usuń produkt z koszyka")]
        public async Task<IActionResult> RemoveFromBasket(int productId, int userId)
        {
            try
            {
                var basket = await _basketRepository.RemoveFromBasket(userId, productId);
                return Ok(basket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd podczas dodawania produktu do koszyka: {ex.Message}");
            }
        }

        [HttpGet("summary")]
        [SwaggerOperation(Summary = "Pokaż koszyk")]
        public async Task<IActionResult> SummaryBasket(int userId)
        {
            try
            {

                var basketProducts = await _basketRepository.SummaryBasket(userId);
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
