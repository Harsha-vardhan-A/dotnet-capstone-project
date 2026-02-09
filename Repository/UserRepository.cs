using capstone_prjct.Entities;
using capstone_prjct.Data;
using Microsoft.EntityFrameworkCore;
using capstone_prjct.DTOs;
using BCrypt.Net;
namespace capstone_prjct.Repository;
public class UserRepository: IUserRepository {
    private readonly AppDbContext context;

    public UserRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await context.User.ToListAsync();
    }
    
    public async Task<User> GetUserByIdAsync(int id)
    {
        var user = await context.User.FindAsync(id);
        return user;
    }
    
    public async Task<User> CreateUserAsync(UserRequest user)
    {
        var newUser = new User
        {
            Name = user.Name,
            Email = user.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
            Role = user.Role,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.User.Add(newUser);
        await context.SaveChangesAsync();
        return newUser;
    }
    
    public async Task<User> UpdateUserAsync(int id, UserRequest user)
    {
        var existingUser = await context.User.FindAsync(id);
        if (existingUser == null)
        {
            return null;
        }
        
        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
        existingUser.Role = user.Role;
        existingUser.UpdatedAt = DateTime.UtcNow;
        
        await context.SaveChangesAsync();
        return existingUser;
    }
    
    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await context.User.FindAsync(id);
        if (user == null)
        {
            return false;
        }
        
        context.User.Remove(user);
        await context.SaveChangesAsync();
        return true;
    }
    public async Task<IEnumerable<User>> GetUserByEmailAsync(string email)
    {
        return await context.User.Where(u => u.Email == email).ToListAsync();
    }
}