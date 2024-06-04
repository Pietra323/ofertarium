using backend.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;

    public OrderController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [HttpPost("transferbaskettohistory")]
    public async Task<IActionResult> TransferBasketToHistory(int paymentCardId)
    {
        try
        {
            int? userId = Auth.GetUserId(HttpContext);
            if (userId == null)
            {
                return Unauthorized();
            }

            await _orderRepository.TransferBasketToHistory(userId.Value, paymentCardId);
            return Ok("Basket has been transferred to history.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    
    [HttpGet("orderhistory")]
    [Authorize]
    [SwaggerOperation(Summary = "Pobierz historię zamówień użytkownika")]
    public async Task<IActionResult> GetUserOrderHistory()
    {
        try
        {
            int? userId = Auth.GetUserId(HttpContext);
            if (userId == null)
            {
                return Unauthorized();
            }

            var orderHistory = await _orderRepository.GetUserOrderHistory(userId.Value);
            return Ok(orderHistory);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}
