using capstone_prjct.Entities;
using capstone_prjct.Data;
using Microsoft.EntityFrameworkCore;
namespace capstone_prjct.Repository;
public class PolicyRepository : IPolicyRepository {
    private readonly AppDbContext context;

    public PolicyRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Policy>> GetAllPoliciesAsync()
    {
        var policies = await this.context.Policy.ToListAsync();
        if (policies == null || policies.Count == 0)
        {
            return Enumerable.Empty<Policy>();
        }   
        return policies;
    }

    public async Task<Policy> GetPolicyByIdAsync(int id)
    {
        var policy = await this.context.Policy.FindAsync(id);
        if (policy == null)
        {
            throw new KeyNotFoundException($"Policy with ID {id} not found.");
        }
        return policy;
    }

    public async Task<IEnumerable<Policy>> SearchPoliciesAsync(int minAmount, int maxAmount)
    {
        var filteredPolicies = this.context.Policy.Where(p => p.PremiumAmount >= minAmount && p.PremiumAmount <= maxAmount);
        if (filteredPolicies == null || !filteredPolicies.Any())
        {
            return Enumerable.Empty<Policy>();
        }
        return await filteredPolicies.ToListAsync();
    }

    public async Task<IEnumerable<Policy>> GetPoliciesByStatusAsync(bool isActive)
    {
        var filteredPolicies = this.context.Policy.Where(p => p.IsActive == isActive).OrderBy(p => p.Id);
        if (filteredPolicies == null || !filteredPolicies.Any())
        {
            return Enumerable.Empty<Policy>();
        }
        return await filteredPolicies.ToListAsync();
    }

    public async Task<Policy> CreatePolicyAsync(Policy policy)
    {
        await this.context.Policy.AddAsync(policy);
        await this.context.SaveChangesAsync();
        return policy;
    }
    public async Task<Policy> EnrollUserInPolicyAsync(int policyId, int userId)
    {
        var userPolicy = await this.context.UserPolicy.AddAsync(new UserPolicy { PolicyId = policyId, UserId = userId, RequestedAt = DateTime.UtcNow, Status = "Pending" });
        await this.context.SaveChangesAsync();
        return await this.context.Policy.FindAsync(policyId);
    }

    public async Task<IEnumerable<Policy>> GetUserEnrolledPoliciesAsync(int userId)
    {
        var enrolledPolicies = await this.context.UserPolicy
            .Where(up => up.UserId == userId)
            .Include(up => up.Policy)
            .Select(up => up.Policy)
            .ToListAsync();
        
        return enrolledPolicies ?? Enumerable.Empty<Policy>();
    }

    public async Task<IEnumerable<UserPolicy>> GetEnrollmentsByStatusAsync(string status)
    {
        var enrollments = await this.context.UserPolicy
            .Where(up => up.Status == status)
            .Include(up => up.User)
            .Include(up => up.Policy)
            .OrderBy(up => up.RequestedAt)
            .ToListAsync();
        
        return enrollments ?? Enumerable.Empty<UserPolicy>();
    }

    public async Task<UserPolicy> ApproveEnrollmentAsync(int enrollmentId)
    {
        var enrollment = await this.context.UserPolicy.FindAsync(enrollmentId);
        if (enrollment == null)
        {
            throw new KeyNotFoundException($"Enrollment with ID {enrollmentId} not found.");
        }
        Console.WriteLine($"Approving enrollment: ID={enrollmentId}, UserId={enrollment.UserId}, Policy{enrollment.ApprovedAt}");
        enrollment.Status = "Approved";
        enrollment.ApprovedAt = DateTime.UtcNow;
        this.context.UserPolicy.Update(enrollment);
        await this.context.SaveChangesAsync();
        
        return enrollment;
    }

    public async Task<UserPolicy> RejectEnrollmentAsync(int enrollmentId)
    {
        var enrollment = await this.context.UserPolicy.FindAsync(enrollmentId);
        if (enrollment == null)
        {
            throw new KeyNotFoundException($"Enrollment with ID {enrollmentId} not found.");
        }
        
        enrollment.Status = "Rejected";
        this.context.UserPolicy.Update(enrollment);
        await this.context.SaveChangesAsync();
        
        return enrollment;
    }

}