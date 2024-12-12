using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.ViewModels.Account
{
    public class ForgetPasswordVM
    {
        [Required, EmailAddress, MaxLength(70)]
        public string Email { get; set; }
        public bool IsSuccess { get; set; }
    }
}
