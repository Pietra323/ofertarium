using backend.Data.Models;
using backend.Data.Models.DataBase;
using backend.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataBase _ctx;
    
    public UserRepository(DataBase ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _ctx.Users.ToListAsync();
    }

    public async Task<IEnumerable<UserDTO>> GetPeopleAsync()
    {
        var users = await _ctx.Users.ToListAsync();

        // Mapowanie użytkowników na użytkowników DTO
        var usersDTO = users.Select(user => new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            isAdmin = user.isAdmin
        });

        return usersDTO;
    }
    
    public async Task<User> GetPeopleByIdAsync(int id)
    {
        return await _ctx.Users.FindAsync(id);
    }
    
    public async Task<User> CreatePersonAsync(User person)
    {
        _ctx.Users.Add(person);
        await _ctx.SaveChangesAsync();
        
        var userId = person.Id;

        AccountSettings NewAccountSettings = new AccountSettings()
        {
            User = person,
            Id = userId
        };
        
        Basket newBasket = new Basket()
        {
            User = person,
            Id = userId
        };

        _ctx.AccountSettings.Add(NewAccountSettings);
        await _ctx.SaveChangesAsync();
        _ctx.Baskets.Add(newBasket);
        await _ctx.SaveChangesAsync();
        return person;
    }
    
    public async Task<User> LoginUser(string username, string password)
    {
        var user = await _ctx.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user.Password == password)
        {
            return user;
        }
        return null;
    }
    
    public async Task UpdatePersonAsync(User person)
    {
        _ctx.Users.Update(person);
        await _ctx.SaveChangesAsync();
    }
    
    public async Task UpdatePasswordAsync(string password)
    {
        var newUser = new User();
        newUser.Password = password;
        _ctx.Users.Update(newUser);
        await _ctx.SaveChangesAsync();
    }
    
    public async Task<UserDTO> DeletePersonAsync(User person)
    {
        var usersDTO = new UserDTO()
        {
            Id = person.Id,
            Name = person.Name,
            LastName = person.LastName,
            Username = person.Username,
            Email = person.Email,
            isAdmin = person.isAdmin
        };
        _ctx.Users.Remove(person);
        await _ctx.SaveChangesAsync();
        return usersDTO;
    }
}