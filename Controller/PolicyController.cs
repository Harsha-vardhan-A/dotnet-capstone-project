
using Microsoft.AspNetCore.Mvc;
using capstone_prjct.Services;
using capstone_prjct.Entities;
using capstone_prjct.Filters;
namespace capstone_prjct.Controller;

[ApiController]
[ServiceFilter(typeof(ResTimeActionFilter))]
[ServiceFilter(typeof(GlobalResponseFilter))]
[ServiceFilter(typeof(AuthenticationFilter))]
[ServiceFilter(typeof(RoleAuthorizationFilter))]
[Route("policy")]
public class PolicyController: ControllerBase
{

    private readonly IPolicyService policyService;
    public PolicyController(IPolicyService policyService)
    {
        this.policyService = policyService;
    }

    [HttpGet]
    [RequireRole("Admin", "User")]
    public async Task<IActionResult> GetAllPoliciesAsync()
    {
        var policies = await policyService.GetAllPoliciesAsync();
        if (policies == null || !policies.Any())
        {
            return NotFound("No policies found.");
        }
        return Ok(policies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPolicyByIdAsync(int id)
    {
        var policy = await policyService.GetPolicyByIdAsync(id);
        if (policy == null)        {
            return NotFound($"Policy with ID {id} not found.");
        }
        return Ok(policy);

    }

    [HttpGet("search", Name = "SearchPolicy")]
    public async Task<IActionResult> SearchPoliciesAsync([FromQuery] int minAmount, [FromQuery] int maxAmount)
    {
        var policies = await policyService.SearchPoliciesAsync(minAmount, maxAmount);
        if (policies == null || !policies.Any())
        {
            return NotFound($"No policies found with premium amount between {minAmount} and {maxAmount}.");
        }
        return Ok(policies);
    }

    [HttpGet("status", Name = "PolicyByStatus")]
    public async Task<IActionResult> PoliciesByStatusAsync([FromQuery] bool isActive)
    {
        var policies = await policyService.GetPoliciesByStatusAsync(isActive);
        if (policies == null || !policies.Any())        {
            return NotFound($"No policies found with status {(isActive ? "active" : "inactive")}.");
        }
        return Ok(policies);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePolicyAsync([FromBody] Policy policy)
    {
        var createdPolicy = await this.policyService.CreatePolicyAsync(policy);
        return Ok(createdPolicy);
    }


    [HttpGet("enrollments")]
    public async Task<IActionResult> GetPolicyEnrollmentsAsync()
    {
        // Logic to retrieve policy enrollments to be implemented
        return Ok("List of policy enrollments.");
    }
    [HttpPost("{policyId}/enroll")]
    public async Task<IActionResult> EnrollUserInPolicyAsync(int policyId)
    {
        // Enrollment logic to be implemented
        var userId = HttpContext.Items["UserId"] as int?;
        Console.WriteLine($"Enrolling user with ID {userId} in policy with ID {policyId}.");
        if (userId == null)        {
            return Unauthorized("User ID not found in context.");
        }
        await policyService.EnrollUserInPolicyAsync(policyId, userId.Value);
        return Ok($"User with ID {userId.Value} enrolled in policy with ID {policyId}.");
    }
}