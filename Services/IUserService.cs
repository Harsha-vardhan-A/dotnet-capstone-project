using capstone_prjct.Entities;
using capstone_prjct.DTOs;
namespace capstone_prjct.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(UserRequest user);
    Task<User> UpdateUserAsync(int id, UserRequest user);
    Task<bool> DeleteUserAsync(int id);
    Task<LoginResponse?> AuthenticateUserAsync(string email, string password);
}