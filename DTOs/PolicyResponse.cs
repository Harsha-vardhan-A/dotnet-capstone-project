namespace capstone_prjct.DTOs;

public class PolicyResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int PremiumAmount { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
