using capstone_prjct.Entities;

namespace capstone_prjct.Services;

public interface IPolicyService
{
    Task<IEnumerable<Policy>> GetAllPoliciesAsync();
    Task<Policy> GetPolicyByIdAsync(int id);
    Task<IEnumerable<Policy>> SearchPoliciesAsync(int minAmount, int maxAmount);
    Task<IEnumerable<Policy>> GetPoliciesByStatusAsync(bool isActive);
    Task<Policy> CreatePolicyAsync(Policy policy);
    Task<Policy> EnrollUserInPolicyAsync(int policyId, int userId);
}