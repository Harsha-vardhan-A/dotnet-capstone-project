
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
    [RequireRole("Admin", "User")]
    public async Task<IActionResult> PoliciesByStatusAsync([FromQuery] bool isActive)
    {
        var policies = await policyService.GetPoliciesByStatusAsync(isActive);
        if (policies == null || !policies.Any())        {
            return NotFound($"No policies found with status {(isActive ? "active" : "inactive")}.");
        }
        return Ok(policies);
    }

    [HttpPost("create")]
    [RequireRole("Admin")]
    public async Task<IActionResult> CreatePolicyAsync([FromBody] Policy policy)
    {
        var createdPolicy = await this.policyService.CreatePolicyAsync(policy);
        return Ok(createdPolicy);
    }

    [HttpPut("admin/{id}/update")]
    [RequireRole("Admin")]
    public async Task<IActionResult> UpdatePolicyAsync(int id, [FromBody] Policy policy)
    {
        try
        {
            var updatedPolicy = await this.policyService.UpdatePolicyAsync(id, policy);
            return Ok(updatedPolicy);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("enrollments")]
    public async Task<IActionResult> GetPolicyEnrollmentsAsync()
    {
        // Logic to retrieve policy enrollments to be implemented
        return Ok("List of policy enrollments.");
    }
    [HttpPost("{policyId}/enroll")]
    [RequireRole("User")]
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

    [HttpGet("my/enrollments")]
    [RequireRole("User")]
    public async Task<IActionResult> GetMyEnrolledPoliciesAsync()
    {
        var userId = HttpContext.Items["UserId"] as int?;
        if (userId == null)
        {
            return Unauthorized("User ID not found in context.");
        }

        var enrolledPolicies = await policyService.GetUserEnrolledPoliciesAsync(userId.Value);
        if (enrolledPolicies == null || !enrolledPolicies.Any())
        {
            return NotFound($"No enrolled policies found for user with ID {userId.Value}.");
        }
        return Ok(enrolledPolicies);
    }

    [HttpGet("admin/enrollments")]
    [RequireRole("Admin")]
    public async Task<IActionResult> GetEnrollmentsByStatusAsync([FromQuery] string status = "Pending")
    {
        var enrollments = await policyService.GetEnrollmentsByStatusAsync(status);
        if (enrollments == null || !enrollments.Any())
        {
            return NotFound($"No enrollments found with status {status}.");
        }
        return Ok(enrollments);
    }

    [HttpPost("admin/enrollments/{id}/approve")]
    [RequireRole("Admin")]
    public async Task<IActionResult> ApproveEnrollmentAsync(int id)
    {
        try
        {
            var enrollment = await policyService.ApproveEnrollmentAsync(id);
            return Ok(new { message = $"Enrollment with ID {id} has been approved.", enrollment });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("admin/enrollments/{id}/reject")]
    [RequireRole("Admin")]
    public async Task<IActionResult> RejectEnrollmentAsync(int id)
    {
        try
        {
            var enrollment = await policyService.RejectEnrollmentAsync(id);
            return Ok(new { message = $"Enrollment with ID {id} has been rejected.", enrollment });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}