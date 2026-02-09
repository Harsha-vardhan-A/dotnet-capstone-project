
using capstone_prjct.Entities;
using capstone_prjct.Repository;
using capstone_prjct.DTOs;

namespace capstone_prjct.Services;

public class PolicyService : IPolicyService
{
    private readonly IPolicyRepository policyRepository;
    public PolicyService(IPolicyRepository policyRepository)
    {
        this.policyRepository = policyRepository;
    }

    public async Task<IEnumerable<PolicyResponse>> GetAllPoliciesAsync()
    {
        var policies = await this.policyRepository.GetAllPoliciesAsync();
        return policies.Select(MapToPolicyResponse);
    }

    public async Task<PolicyResponse> GetPolicyByIdAsync(int id)
    {
        var policy = await this.policyRepository.GetPolicyByIdAsync(id);
        return MapToPolicyResponse(policy);
    }

    public async Task<IEnumerable<PolicyResponse>> SearchPoliciesAsync(int minAmount, int maxAmount)
    {
        var policies = await this.policyRepository.SearchPoliciesAsync(minAmount, maxAmount);
        return policies.Select(MapToPolicyResponse);
    }

    public async Task<IEnumerable<PolicyResponse>> GetPoliciesByStatusAsync(bool isActive)
    {
        var policies = await this.policyRepository.GetPoliciesByStatusAsync(isActive);
        return policies.Select(MapToPolicyResponse);
    }

    public async Task<PolicyResponse> CreatePolicyAsync(PolicyRequest policyRequest)
    {
        var policy = MapToPolicyEntity(policyRequest);
        var createdPolicy = await this.policyRepository.CreatePolicyAsync(policy);
        return MapToPolicyResponse(createdPolicy);
    }

    public async Task<PolicyResponse> EnrollUserInPolicyAsync(int policyId, int userId)
    {
        var policy = await this.policyRepository.GetPolicyByIdAsync(policyId);
        if (policy == null)        {
            throw new KeyNotFoundException($"Policy with ID {policyId} not found.");
        }
        var enrolledPolicy = await this.policyRepository.EnrollUserInPolicyAsync(policyId, userId);
        return MapToPolicyResponse(enrolledPolicy);
    }

    public async Task<IEnumerable<PolicyResponse>> GetUserEnrolledPoliciesAsync(int userId)
    {
        var policies = await this.policyRepository.GetUserEnrolledPoliciesAsync(userId);
        return policies.Select(MapToPolicyResponse);
    }

    public async Task<IEnumerable<UserPolicyResponse>> GetEnrollmentsByStatusAsync(string status)
    {
        var enrollments = await this.policyRepository.GetEnrollmentsByStatusAsync(status);
        return enrollments.Select(MapToUserPolicyResponse);
    }

    public async Task<UserPolicyResponse> ApproveEnrollmentAsync(int enrollmentId)
    {
        var enrollment = await this.policyRepository.ApproveEnrollmentAsync(enrollmentId);
        return MapToUserPolicyResponse(enrollment);
    }

    public async Task<UserPolicyResponse> RejectEnrollmentAsync(int enrollmentId)
    {
        var enrollment = await this.policyRepository.RejectEnrollmentAsync(enrollmentId);
        return MapToUserPolicyResponse(enrollment);
    }

    public async Task<PolicyResponse> UpdatePolicyAsync(int id, PolicyRequest policyRequest)
    {
        var existingPolicy = await this.policyRepository.GetPolicyByIdAsync(id);
        if (existingPolicy == null)        {
            throw new KeyNotFoundException($"Policy with ID {id} not found.");
        }
        var policy = MapToPolicyEntity(policyRequest);
        var updatedPolicy = await this.policyRepository.UpdatePolicyAsync(id, policy);
        return MapToPolicyResponse(updatedPolicy);
    }

    private PolicyResponse MapToPolicyResponse(Policy policy)
    {
        return new PolicyResponse
        {
            Id = policy.Id,
            Name = policy.Name,
            PremiumAmount = policy.PremiumAmount,
            Description = policy.Description,
            IsActive = policy.IsActive,
            CreatedDate = policy.CreatedDate,
            UpdatedDate = policy.UpdatedDate
        };
    }

    private Policy MapToPolicyEntity(PolicyRequest request)
    {
        return new Policy
        {
            Name = request.Name,
            PremiumAmount = request.PremiumAmount,
            Description = request.Description,
            IsActive = request.IsActive
        };
    }

    private UserPolicyResponse MapToUserPolicyResponse(UserPolicy userPolicy)
    {
        return new UserPolicyResponse
        {
            Id = userPolicy.Id,
            UserId = userPolicy.UserId,
            PolicyId = userPolicy.PolicyId,
            RequestedAt = userPolicy.RequestedAt,
            ApprovedAt = userPolicy.ApprovedAt,
            Status = userPolicy.Status,
            User = userPolicy.User != null ? MapToUserResponse(userPolicy.User) : null,
            Policy = userPolicy.Policy != null ? MapToPolicyResponse(userPolicy.Policy) : null
        };
    }

    private UserResponse MapToUserResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

}