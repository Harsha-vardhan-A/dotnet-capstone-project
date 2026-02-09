using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_prjct.Entities
{
    [Table("policy")]
    public class Policy
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Column("policy_name")]
        [Required(ErrorMessage = "Policy name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Policy name must be between 3 and 100 characters")]
        public string Name { get; set; }
        
        [Column("premium_amount")]
        [Range(1, 100000, ErrorMessage = "Premium amount must be between 1 and 100000")]
        public int PremiumAmount { get; set; }
        
        [Column("policy_description")]
        [Required(ErrorMessage = "Policy description is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters")]
        public string Description { get; set; }
        
        [Column("is_active")]
        public bool IsActive { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedDate { get; set; }
        
        [Column("updated_at")]
        public DateTime UpdatedDate { get; set; }
    }
}