
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

    public Task<Policy> EnrollUserInPolicyAsync(int policyId, int userId)
    {
        throw new NotImplementedException();
    }

}