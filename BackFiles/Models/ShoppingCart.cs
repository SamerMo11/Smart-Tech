using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string User_Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual ICollection<CartDetails> CartDetails { get; set; }
    }
}
