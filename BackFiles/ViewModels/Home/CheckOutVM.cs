using E_CommerceWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.ViewModels.Home
{
    public class CheckOutVM
    {
        public ShoppingCartVM? ShoppingCart { get; set; }
        [Display(Name = "City")]
        [Required]
        public int City_Id { get; set; }
        [MaxLength(100)]
        [Display(Name = "Address")]
        [Required]
        public string Address { get; set; }
        [Display(Name = "States")]
        public List<state>? States { get; set; }
        public ApplicationUser? UESR { get; set; }
        [Display(Name = "Phone"), MaxLength(12)]
        [Required]
        public string Phone { get; set; }
        public string? code { get; set; }
        [Required(ErrorMessage = "Choose Payment Method ...."), Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }
    }
}
