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
    Task<IEnumerable<Policy>> GetUserEnrolledPoliciesAsync(int userId);
    Task<IEnumerable<UserPolicy>> GetEnrollmentsByStatusAsync(string status);
    Task<UserPolicy> ApproveEnrollmentAsync(int enrollmentId);
    Task<UserPolicy> RejectEnrollmentAsync(int enrollmentId);
    Task<Policy> UpdatePolicyAsync(int id, Policy policy);
}