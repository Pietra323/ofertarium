using backend.Data.Models;

namespace backend.Data.Repositories.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetPeopleAsync();
    Task<User> GetPeopleByIdAsync(int id);
    Task<User> CreatePersonAsync(User person);
    Task UpdatePersonAsync(User person);
    Task DeletePersonAsync(User person);
}