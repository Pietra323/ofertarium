using backend.Data.Models;
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
        return person;
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