using System.ComponentModel.DataAnnotations;

namespace capstone_prjct.DTOs;
public class UserRequest
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Role is required")]
    [RegularExpression("Admin|User", ErrorMessage = "Role must be either 'Admin' or 'User'")]
    public string Role { get; set; }
}