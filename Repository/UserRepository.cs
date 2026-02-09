using capstone_prjct.Entities;
using capstone_prjct.Data;
using Microsoft.EntityFrameworkCore;
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
    
    public async Task<User> CreateUserAsync(User user)
    {
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        context.User.Add(user);
        await context.SaveChangesAsync();
        return user;
    }
    
    public async Task<User> UpdateUserAsync(int id, User user)
    {
        var existingUser = await context.User.FindAsync(id);
        if (existingUser == null)
        {
            return null;
        }
        
        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.PasswordHash = user.PasswordHash;
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