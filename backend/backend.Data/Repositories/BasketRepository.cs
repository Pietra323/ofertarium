using backend.Data.Models;
using backend.Data.Models.DataBase;
using backend.Data.Models.ManyToManyConnections;
using backend.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly DataBase _ctx;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;

    public BasketRepository(DataBase ctx, IUserRepository userRepository, IProductRepository productRepository)
    {
        _ctx = ctx;
        _userRepository = userRepository;
        _productRepository = productRepository;
    }

    public async Task<Basket> AddToBasket(int userId, int productId)
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
            // Product is already in the basket, increase its quantity
            existingBasketProduct.quantity += 1;
        }
        else
        {
            // Product is not in the basket yet, add a new one
            var productDTO = await _productRepository.GetProductById(productId);
            var product2 = await _ctx.Products.FindAsync(productDTO.IdProduct);
            if (productDTO != null)
            {
                var basketProduct = new BasketProduct
                {
                    product = product2, 
                    Basket = basket, 
                    quantity = 1
                };
                basket.BasketProducts.Add(basketProduct);
            }
            else
            {
                // Handle case when product is not found
            }
        }
    
        await _ctx.SaveChangesAsync();
        return basket;
    }


    public async Task<Basket> RemoveFromBasket(int userId, int productId)
    {
        var user = await _userRepository.GetPeopleByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        var basket = user.Basket;
        if (basket == null || basket.BasketProducts == null)
        {
            throw new ArgumentException("Basket or basket products not found");
        }

        var existingBasketProduct = basket.BasketProducts.FirstOrDefault(bp => bp.ProductId == productId);

        if (existingBasketProduct != null)
        {
            if (existingBasketProduct.quantity > 1)
            {
                // Produkt już jest w koszyku, zmniejsz jego ilość
                existingBasketProduct.quantity -= 1;
            }
            else
            {
                // Produkt zostanie kompletnie usunięty z koszyka
                _ctx.BasketProducts.Remove(existingBasketProduct);
            }

            await _ctx.SaveChangesAsync();
        }
        return basket;
    }

    public async Task<IEnumerable<BasketProductDto>> SummaryBasket(int userId)
    {
        var basket = await _ctx.Baskets
            .Include(b => b.BasketProducts)
            .ThenInclude(bp => bp.product)
            .FirstOrDefaultAsync(b => b.Id == userId);

        if (basket == null)
        {
            return null; // Lub rzuć odpowiedni wyjątek, np. NotFoundException
        }

        var basketProducts = basket.BasketProducts.Select(bp => new BasketProductDto
        {
            ProductId = bp.ProductId,
            ProductName = bp.product.ProductName,
            Quantity = bp.quantity, // Użycie domyślnej wartości 0, jeśli quantity jest null
            Price = bp.product.Price,
            TotalPrice = (bp.quantity) * (bp.product.Price)
        }).ToList();

        return basketProducts;
    }

    public class BasketProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
