
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

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
    {
        var users = await this.userRepository.GetAllUsersAsync();
        return users.Select(MapToUserResponse);
    }

    public async Task<UserResponse> GetUserByIdAsync(int id)
    {
        var user = await this.userRepository.GetUserByIdAsync(id);
        return MapToUserResponse(user);
    }

    public async Task<UserResponse> CreateUserAsync(UserRequest user)
    {
        var createdUser = await this.userRepository.CreateUserAsync(user);
        return MapToUserResponse(createdUser);
    }

    public async Task<UserResponse> UpdateUserAsync(int id, UserRequest user)
    {
        var updatedUser = await this.userRepository.UpdateUserAsync(id, user);
        return MapToUserResponse(updatedUser);
    }

    public Task<bool> DeleteUserAsync(int id)
    {
        return this.userRepository.DeleteUserAsync(id);
    }
    
    public async Task<LoginResponse?> AuthenticateUserAsync(string email, string password)
    {
        var users = await this.userRepository.GetUserByEmailAsync(email);
        var user = users.FirstOrDefault(u => u.Email == email);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
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

    private UserResponse MapToUserResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

}