using E_CommerceWeb.ViewModels.Email;

namespace E_CommerceWeb.Repository.Interface
{
    public interface IEmailRepo
    {
        //Email Confirmation Method
        Task SendEmailConfirmation(UserEmailOptions emailOptions);




        //Forget Password Confirmation Method
        Task SendForgetPasswordConfirmation(UserEmailOptions emailOptions);




        //Send Code On the Email of User ==> Check Out Page
        Task SendDiscountCodeOnEmailAsync(string userId);
    }
}
