
using capstone_prjct.Entities;
using capstone_prjct.Repository;
using capstone_prjct.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace capstone_prjct.Services;

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly IConfiguration configuration;

    public UserService(IUserRepository userRepository, IConfiguration configuration)
    {
        this.userRepository = userRepository;
        this.configuration = configuration;
    }

    public Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return this.userRepository.GetAllUsersAsync();
    }

    public Task<User> GetUserByIdAsync(int id)
    {
        return this.userRepository.GetUserByIdAsync(id);
    }

    public Task<User> CreateUserAsync(User user)
    {
        return this.userRepository.CreateUserAsync(user);
    }

    public Task<User> UpdateUserAsync(int id, User user)
    {
        return this.userRepository.UpdateUserAsync(id, user);
    }

    public Task<bool> DeleteUserAsync(int id)
    {
        return this.userRepository.DeleteUserAsync(id);
    }
    public async Task<LoginResponse?> AuthenticateUserAsync(string email, string password)
    {
        var users = await this.userRepository.GetUserByEmailAsync(email);
        var user = users.FirstOrDefault(u => u.Email == email);
        
        if (user == null || user.PasswordHash != password) // In real applications, use proper password hashing
        {
            return null;
        }

        // Generate JWT token
        var token = GenerateJwtToken(user);
        var expiryMinutes = configuration.GetValue<int>("Jwt:ExpiryMinutes");
        
        return new LoginResponse
        {
            Token = token,
            Email = user.Email,
            Name = user.Name,
            Role = user.Role,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes)
        };
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
        };
        
        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpiryMinutes")),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}