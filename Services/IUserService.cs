using capstone_prjct.Entities;
using capstone_prjct.DTOs;
namespace capstone_prjct.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(int id, User user);
    Task<bool> DeleteUserAsync(int id);
    Task<LoginResponse?> AuthenticateUserAsync(string email, string password);
}