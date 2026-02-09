using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using capstone_prjct.Entities;
using capstone_prjct.Services;
using capstone_prjct.DTOs;
using capstone_prjct.Filters;
namespace capstone_prjct.Controller;

[ApiController]
[ServiceFilter(typeof(ResTimeActionFilter))]
[ServiceFilter(typeof(GlobalResponseFilter))]
[Route("user")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }
    
    [HttpPost("create")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser([FromBody] UserRequest user)
    {
        // Implementation for creating a user goes here
        var createdUser = await this.userService.CreateUserAsync(user);
        return Ok(createdUser);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByIdAsync(int id)
    {
        // Implementation for retrieving a user by ID goes here
        var user = await this.userService.GetUserByIdAsync(id);
        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        // Implementation for retrieving all users goes here
        var users = await this.userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] UserRequest user)
    {
        // Implementation for updating a user goes here
        var updatedUser = await this.userService.UpdateUserAsync(id, user);
        return Ok(updatedUser);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        // Implementation for deleting a user goes here
        var result = await this.userService.DeleteUserAsync(id);
        if (!result)
        {
            return NotFound($"User with ID {id} not found.");
        }
        return Ok("User deleted successfully.");
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await this.userService.AuthenticateUserAsync(request.Email, request.Password);
        if (response == null)
        {
            return Unauthorized("Invalid email or password.");
        }
        return Ok(response);
    }

}
