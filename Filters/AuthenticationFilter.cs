using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using capstone_prjct.Repository;

namespace capstone_prjct.Filters;

public class AuthenticationFilter : IAsyncActionFilter
{
    private readonly IUserRepository _userRepository;

    public AuthenticationFilter(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Check if the Authorization header exists
        if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                Status = false,
                Message = "Authorization header is missing",
                Timestamp = DateTime.UtcNow
            });
            return;
        }

        var token = authHeader.ToString();
        
        // Check if the token starts with "Bearer "
        if (!token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                Status = false,
                Message = "Invalid authorization header format. Expected 'Bearer {token}'",
                Timestamp = DateTime.UtcNow
            });
            return;
        }

        // Extract the token
        var jwtToken = token.Substring("Bearer ".Length).Trim();

        try
        {
            // Parse the JWT token
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

            if (jsonToken == null)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    Status = false,
                    Message = "Invalid token format",
                    Timestamp = DateTime.UtcNow
                });
                return;
            }

            // Extract user information from token claims
            var userIdClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var emailClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var roleClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    Status = false,
                    Message = "User ID not found in token",
                    Timestamp = DateTime.UtcNow
                });
                return;
            }

            // Retrieve user from database
            var userId = int.Parse(userIdClaim);
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    Status = false,
                    Message = "User not found",
                    Timestamp = DateTime.UtcNow
                });
                return;
            }

            // Store user information in HttpContext.Items for use in controllers
            context.HttpContext.Items["UserId"] = user.Id;
            context.HttpContext.Items["UserEmail"] = user.Email;
            context.HttpContext.Items["UserName"] = user.Name;
            context.HttpContext.Items["UserRole"] = user.Role;
            context.HttpContext.Items["User"] = user;

            // Continue to the next filter or action
            await next();
        }
        catch (Exception ex)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                Status = false,
                Message = $"Token validation failed: {ex.Message}",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
