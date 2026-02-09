using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_prjct.Entities;

[Table("user_policy")]
public class UserPolicy
{
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("policy_id")]
    public int PolicyId { get; set; }

    [Column("requested_at")]
    public DateTime RequestedAt { get; set; }

    [Column("approved_at")]
    public DateTime? ApprovedAt { get; set; }

    [Column("status")]
    public string Status { get; set; }
    
    public User User { get; set; }
    public Policy Policy { get; set; }
}