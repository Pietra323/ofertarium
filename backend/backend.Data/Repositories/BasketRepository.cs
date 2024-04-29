using backend.Data.Models;
using backend.Data.Models.DataBase;
using backend.Data.Models.ManyToManyConnections;
using backend.Data.Repositories.Interfaces;

namespace backend.Data.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly DataBase _ctx;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;

    public BasketRepository(DataBase ctx, IUserRepository userRepository ,IProductRepository productRepository)
    {
        _ctx = ctx;
        _userRepository = userRepository;
        _productRepository = productRepository;
    }

    public async Task AddToBasket(int userId, int productId)
    {
        var user = await _userRepository.GetPeopleByIdAsync(userId);
        var basket = user.Basket;
        
        if (basket.BasketProducts == null)
        {
            basket.BasketProducts = new List<BasketProduct>();
        }

        var existingBasketProduct = basket.BasketProducts.FirstOrDefault(bp => bp.ProductId == productId);

        if (existingBasketProduct != null)
        {
            // Produkt już jest w koszyku, zwiększ jego ilość
            existingBasketProduct.quantity += 1;
        }
        else
        {
            // Produkt nie jest jeszcze w koszyku, dodaj nowy
            var product = await _productRepository.GetProductById(productId);
            var basketProduct = new BasketProduct { product = product, Basket = basket, quantity = 1};
            if (basket.BasketProducts == null)
            {
                basket.BasketProducts = new List<BasketProduct>();
            }

            basket.BasketProducts.Add(basketProduct);
        }

        // Zapisz zmiany w bazie danych
        await _ctx.SaveChangesAsync();
    }

}