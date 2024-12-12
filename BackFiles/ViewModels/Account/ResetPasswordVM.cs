using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.ViewModels.Account
{
    public class ResetPasswordVM
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        [Required, DataType(DataType.Password), MaxLength(100)]
        public string NewPassword { get; set; }
        [Required, DataType(DataType.Password), MaxLength(100), Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
        public bool IsReseted { get; set; }
    }
}
