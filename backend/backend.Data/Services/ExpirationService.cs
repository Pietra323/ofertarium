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

    private void UpdateExpirationTimes(object state)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DataBase>();

            // Sprawdź typ argumentu explicite
            var OnSaleProducts = context.OnSales
                .Where(p => p.ExpirationTime.HasValue)
                .ToList<OnSale>();


            foreach (var OnSaleProduct in OnSaleProducts)
            {
                OnSaleProduct.ExpirationTime = OnSaleProduct.ExpirationTime.Value.AddMinutes(-1);

                if (OnSaleProduct.ExpirationTime <= DateTime.Now)
                {
                    context.OnSales.Remove(OnSaleProduct);
                    var product = context.Products.Find(OnSaleProduct.ProductId);
                    product.Price = OnSaleProduct.OldPrice;
                    context.Products.Update(product);
                }
            }

            context.SaveChangesAsync();
        }

        _logger.LogInformation("Updated expiration times for products.");
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