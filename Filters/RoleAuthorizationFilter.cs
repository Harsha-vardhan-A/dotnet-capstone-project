using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace capstone_prjct.Filters;

public class RoleAuthorizationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Check if the endpoint has RequireRole attribute
        var endpoint = context.ActionDescriptor.EndpointMetadata
            .OfType<RequireRoleAttribute>()
            .FirstOrDefault();

        // If no role requirement, proceed
        if (endpoint == null)
        {
            await next();
            return;
        }

        // Check if user is authenticated (should be set by AuthenticationFilter)
        if (!context.HttpContext.Items.ContainsKey("UserRole"))
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                Status = false,
                Message = "User is not authenticated",
                Timestamp = DateTime.UtcNow
            });
            return;
        }

        var userRole = context.HttpContext.Items["UserRole"]?.ToString();

        if (string.IsNullOrEmpty(userRole))
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                Status = false,
                Message = "User role not found",
                Timestamp = DateTime.UtcNow
            });
            return;
        }

        // Check if user's role matches any of the required roles
        var requiredRoles = endpoint.Roles;
        var hasRequiredRole = requiredRoles.Any(role => 
            role.Equals(userRole, StringComparison.OrdinalIgnoreCase));

        if (!hasRequiredRole)
        {
            context.Result = new ObjectResult(new
            {
                Status = false,
                Message = $"Access denied. Required roles: {string.Join(", ", requiredRoles)}",
                UserRole = userRole,
                Timestamp = DateTime.UtcNow
            })
            {
                StatusCode = 403 // Forbidden
            };
            return;
        }

        // User has required role, proceed
        await next();
    }
}
