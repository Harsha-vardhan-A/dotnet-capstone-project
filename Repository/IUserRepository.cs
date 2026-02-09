using capstone_prjct.Entities;
namespace capstone_prjct.Repository;
public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(int id, User user);
    Task<bool> DeleteUserAsync(int id);
    Task<IEnumerable<User>> GetUserByEmailAsync(string email);
}