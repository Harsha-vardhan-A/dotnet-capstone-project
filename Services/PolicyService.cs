
using capstone_prjct.Entities;
using capstone_prjct.Repository;

namespace capstone_prjct.Services;

public class PolicyService : IPolicyService
{
    private readonly IPolicyRepository policyRepository;
    public PolicyService(IPolicyRepository policyRepository)
    {
        this.policyRepository = policyRepository;
    }

    public Task<IEnumerable<Policy>> GetAllPoliciesAsync()
    {
        return this.policyRepository.GetAllPoliciesAsync();
    }

    public Task<Policy> GetPolicyByIdAsync(int id)
    {
        return this.policyRepository.GetPolicyByIdAsync(id);
    }

    public Task<IEnumerable<Policy>> SearchPoliciesAsync(int minAmount, int maxAmount)
    {
        return this.policyRepository.SearchPoliciesAsync(minAmount, maxAmount);
    }

    public Task<IEnumerable<Policy>> GetPoliciesByStatusAsync(bool isActive)
    {
        return this.policyRepository.GetPoliciesByStatusAsync(isActive);
    }

    public Task<Policy> CreatePolicyAsync(Policy policy)
    {
        return this.policyRepository.CreatePolicyAsync(policy);
    }

    public async Task<Policy> EnrollUserInPolicyAsync(int policyId, int userId)
    {
        var policy = await this.policyRepository.GetPolicyByIdAsync(policyId);
        if (policy == null)        {
            throw new KeyNotFoundException($"Policy with ID {policyId} not found.");
        }
        return await this.policyRepository.EnrollUserInPolicyAsync(policyId, userId);
    }

    public Task<IEnumerable<Policy>> GetUserEnrolledPoliciesAsync(int userId)
    {
        return this.policyRepository.GetUserEnrolledPoliciesAsync(userId);
    }

    public Task<IEnumerable<UserPolicy>> GetEnrollmentsByStatusAsync(string status)
    {
        return this.policyRepository.GetEnrollmentsByStatusAsync(status);
    }

    public Task<UserPolicy> ApproveEnrollmentAsync(int enrollmentId)
    {
        return this.policyRepository.ApproveEnrollmentAsync(enrollmentId);
    }

    public Task<UserPolicy> RejectEnrollmentAsync(int enrollmentId)
    {
        return this.policyRepository.RejectEnrollmentAsync(enrollmentId);
    }

    public async Task<Policy> UpdatePolicyAsync(int id, Policy policy)
    {
        var existingPolicy = await this.policyRepository.GetPolicyByIdAsync(id);
        if (existingPolicy == null)        {
            throw new KeyNotFoundException($"Policy with ID {id} not found.");
        }
        return await this.policyRepository.UpdatePolicyAsync(id, policy);
    }

}