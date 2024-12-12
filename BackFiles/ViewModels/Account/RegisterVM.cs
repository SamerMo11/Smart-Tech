using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.ViewModels.Account
{
    public class RegisterVM
    {
        [Display(Name = "First")]
        [Required(ErrorMessage = "*Required*")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*Required*")]
        public string LastName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "*Required*")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*Required*")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "*Not Matched*")]
        [Required(ErrorMessage = "*Required*")]
        public string ConfirmPassword { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*Required*")]
        public string Number { get; set; }
        [Required(ErrorMessage = "*Required*")]
        public bool Agree { get; set; }
    }
}
