using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.Models
{
    public class Favourite
    {
        [ForeignKey("ApplicationUser")]
        [Display(Name = "User")]
        public string User_Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("ProductVariant")]
        [Display(Name = "ProductVariant")]
        public int ProductIn_Id { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }
    }
}
