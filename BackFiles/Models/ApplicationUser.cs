using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ImagePath { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("DiscountCode")]
        public int? DisCodeId { get; set; }
        public virtual DiscountCode DiscountCode { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}
