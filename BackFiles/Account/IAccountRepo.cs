using E_CommerceWeb.Models;
using E_CommerceWeb.ViewModels.Account;
using E_CommerceWeb.ViewModels.Account.DashData;
using Microsoft.AspNetCore.Identity;

namespace E_CommerceWeb.Repository.Interface
{
    public interface IAccountRepo
    {
        //Create Account
        Task<IdentityResult> CreateUserAsync(RegisterVM model);


        //Create Email Confirmation Token
        Task GenerateEmailConfirmationTokenAsync(ApplicationUser user);


        //Ensure Email Confirmation
        Task<IdentityResult> EmailConfirmAsync(ApplicationUser user, string token);


        //Get User By Id
        Task<ApplicationUser> GetUserById(string userid);


        //Return User By Email
        Task<ApplicationUser> GetUserByEmail(string Email);



        //Accept login 
        Task<SignInResult> LoginAsync(LoginVM model);



        // Method for Logout 
        Task LogoutAsync();




        //make token for Forget Password 
        Task GenerateForgetPasswordTokenAsync(ApplicationUser user);


        //Make Reset Password Method
        Task<IdentityResult> ResetPasswordConfirmAsync(ApplicationUser user, string token, string Password);



        //Get User Dashboard Data ==> Main Page {First Page}
        Task<AccountVM> GetAccountDataAsync();





        //Get User Dashboard Data ==> WishList Page {Third Page}
        Task<dynamic> GetWishListDataAsync();


        //Get User Dashboard Data ==> Order Page {Second Page}
        Task<List<AccountOrderVM>> GetOrderAndDetailsAsync();



        //Get User Dashboard Data ==> User Data {Fourth Page}

        Task<dynamic> GetUserDataAsync();




        //Update User Image 
        Task<dynamic> UpdateUserImageAsync(IFormFile file);




        //update User Info
        Task UpdateUserData(string FirstName, string LastName, string Email, string Phone);


    }
}
