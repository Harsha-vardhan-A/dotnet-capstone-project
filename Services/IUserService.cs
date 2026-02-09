using capstone_prjct.DTOs;
namespace capstone_prjct.Services;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetAllUsersAsync();
    Task<UserResponse> GetUserByIdAsync(int id);
    Task<UserResponse> CreateUserAsync(UserRequest user);
    Task<UserResponse> UpdateUserAsync(int id, UserRequest user);
    Task<bool> DeleteUserAsync(int id);
    Task<LoginResponse?> AuthenticateUserAsync(string email, string password);
}