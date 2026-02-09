using System.ComponentModel.DataAnnotations;

namespace capstone_prjct.DTOs;

public class PolicyRequest
{
    [Required(ErrorMessage = "Policy name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Policy name must be between 3 and 100 characters")]
    public string Name { get; set; }
    
    [Range(1, 100000, ErrorMessage = "Premium amount must be between 1 and 100000")]
    public int PremiumAmount { get; set; }
    
    [Required(ErrorMessage = "Policy description is required")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters")]
    public string Description { get; set; }
    
    public bool IsActive { get; set; }
}
