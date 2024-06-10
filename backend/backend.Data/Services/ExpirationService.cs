using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using backend.Data.Models;
using backend.Data.Models.DataBase;
using Microsoft.EntityFrameworkCore;

public class ExpirationService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ExpirationService> _logger;

    public ExpirationService(IServiceScopeFactory scopeFactory, ILogger<ExpirationService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(UpdateExpirationTimes, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        _logger.LogInformation("Started the expiration timer service.");
        return Task.CompletedTask;
    }

    private async void UpdateExpirationTimes(object state)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DataBase>();

            var OnSaleProducts = await context.OnSales
                .Where(p => p.ExpirationTime.HasValue)
                .ToListAsync();

            foreach (var OnSaleProduct in OnSaleProducts)
            {
                _logger.LogInformation($"Checking product {OnSaleProduct.ProductId} with expiration time {OnSaleProduct.ExpirationTime.Value.ToString("o")} UTC");

                if (OnSaleProduct.ExpirationTime <= DateTime.Now)
                {
                    _logger.LogInformation($"Product {OnSaleProduct.ProductId} has expired. Removing on-sale status.");

                    var product = await context.Products.FindAsync(OnSaleProduct.ProductId);
                    if (product != null)
                    {
                        _logger.LogInformation($"Reverting price of product {OnSaleProduct.ProductId} from {product.Price} to {OnSaleProduct.OldPrice}");
                        product.Price = OnSaleProduct.OldPrice;
                        context.Products.Update(product);
                    }

                    context.OnSales.Remove(OnSaleProduct);
                    _logger.LogInformation($"Removed on-sale status for product {OnSaleProduct.ProductId}");
                }
                else
                {
                    _logger.LogInformation($"Product {OnSaleProduct.ProductId} has not expired yet. Expiration time: {OnSaleProduct.ExpirationTime.Value.ToString("o")} UTC, Current time: {DateTime.UtcNow.ToString("o")} UTC");
                }
            }

            await context.SaveChangesAsync();
            _logger.LogInformation("Finished updating expiration times for products.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}