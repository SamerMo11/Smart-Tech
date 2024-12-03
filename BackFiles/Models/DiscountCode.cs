using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.Models
{
    public class DiscountCode
    {
        [Key]
        public int Id { get; set; }
        public string? Code { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public bool IsUsed { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
