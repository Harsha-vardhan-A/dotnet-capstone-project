using capstone_prjct.DTOs;

namespace capstone_prjct.Services;

public interface IPolicyService
{
    Task<IEnumerable<PolicyResponse>> GetAllPoliciesAsync();
    Task<PolicyResponse> GetPolicyByIdAsync(int id);
    Task<IEnumerable<PolicyResponse>> SearchPoliciesAsync(int minAmount, int maxAmount);
    Task<IEnumerable<PolicyResponse>> GetPoliciesByStatusAsync(bool isActive);
    Task<PolicyResponse> CreatePolicyAsync(PolicyRequest policy);
    Task<PolicyResponse> EnrollUserInPolicyAsync(int policyId, int userId);
    Task<IEnumerable<PolicyResponse>> GetUserEnrolledPoliciesAsync(int userId);
    Task<IEnumerable<UserPolicyResponse>> GetEnrollmentsByStatusAsync(string status);
    Task<UserPolicyResponse> ApproveEnrollmentAsync(int enrollmentId);
    Task<UserPolicyResponse> RejectEnrollmentAsync(int enrollmentId);
    Task<PolicyResponse> UpdatePolicyAsync(int id, PolicyRequest policy);
}