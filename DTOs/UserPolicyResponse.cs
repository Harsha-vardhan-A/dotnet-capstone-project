namespace capstone_prjct.DTOs;

public class UserPolicyResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PolicyId { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string Status { get; set; }
    public UserResponse User { get; set; }
    public PolicyResponse Policy { get; set; }
}
