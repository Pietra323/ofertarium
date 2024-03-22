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

    public async Task<IEnumerable<User>> GetPeopleAsync()
    {
        var users = await _ctx.Users.ToListAsync();
        return users;
    }
    
    public async Task<User> GetPeopleByIdAsync(int id)
    {
        return await _ctx.Users.FindAsync(id);
    }
    
    public async Task<User> CreatePersonAsync(User person)
    {
        _ctx.Users.Add(person);
        await _ctx.SaveChangesAsync();

        AccountSettings NewAccountSettings = new AccountSettings()
        {
            User = person,
            Id = person.Id
        };

        _ctx.AccountSettings.Add(NewAccountSettings);
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
    
    public async Task DeletePersonAsync(User person)
    {
        _ctx.Users.Remove(person);
        await _ctx.SaveChangesAsync();
    }
}