using backend.Data.Models;
using backend.Data.Models.DataBase;
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
        var user = _userRepository.GetPeopleByIdAsync(userId);
        var produ
    }
}