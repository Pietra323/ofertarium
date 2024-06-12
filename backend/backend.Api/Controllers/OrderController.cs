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

    [HttpPost("makeOrder")]
    [SwaggerOperation(Summary = "Złóż zamówienie")]
    public async Task<IActionResult> TransferBasketToHistory(int paymentCardId, int userId)
    {
        try
        {
            await _orderRepository.TransferBasketToHistory(userId, paymentCardId);
            return Ok("Basket has been transferred to history.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    
    [HttpGet("orderhistory")]
    [SwaggerOperation(Summary = "Pobierz historię zamówień użytkownika")]
    public async Task<IActionResult> GetUserOrderHistory(int userId)
    {
        try
        {
            var orderHistory = await _orderRepository.GetUserOrderHistory(userId);
            return Ok(orderHistory);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    
    [HttpGet("/pdf")]
    [SwaggerOperation(Summary = "Pobierz zamówienie jako PDF")]
    public async Task<IActionResult> GetOrderPdf(int orderid)
    {
        try
        {
            var pdf = await _orderRepository.GenerateOrderPdfAsync(orderid);
            return File(pdf, "application/pdf", $"Order_{orderid}.pdf");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}
