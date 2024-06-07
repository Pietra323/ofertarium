using backend.Data.Models;

namespace backend.Data.Repositories.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<UserDTO>> GetPeopleAsync();
    Task<User> GetPeopleByIdAsync(int id);
    Task<User> CreatePersonAsync(User person);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task UpdatePersonAsync(User person);
    Task<UserDTO> DeletePersonAsync(User person);
    Task<User> LoginUser(string username, string password);
    Task UpdatePasswordAsync(string password);
}