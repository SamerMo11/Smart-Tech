using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.ViewModels.Account
{
    public class LoginVM
    {
        [EmailAddress]
        [Required(ErrorMessage = "Enter Your Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Your Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
        public string? returnUrl { get; set; }
    }
}
