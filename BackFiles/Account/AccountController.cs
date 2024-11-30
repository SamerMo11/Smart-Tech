using E_CommerceWeb.Models;
using E_CommerceWeb.Repository.Interface;
using E_CommerceWeb.ViewModels.Account;
using E_CommerceWeb.ViewModels.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountRepo _accountRepo;

        public AccountController(SignInManager<ApplicationUser> signInManager,
                                 IAccountRepo accountRepo)
        {
            _signInManager = signInManager;
            _accountRepo = accountRepo;
        }


        /// <summary>
        ///         //asp-route-returnUrl="@Context.Request.Path"     ====>>       In Login Link
        /// </summary>
        /// <returns></returns>




        //Return the User Dashboard Page
        public IActionResult UserDash()
        {
            return View();
        }


        //Method For Register View GET
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Home", "Home");
            }
            LoginVM login = new LoginVM
            {
                returnUrl = returnUrl,
            };
            var model = new LoginSignVM();
            model.LoginVM = login;
            return View(model);
        }



        //Method For Register View Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM newuser)
        {
            var LoginSign = new LoginSignVM();
            if (ModelState.IsValid)
            {
                //create user
                var result = await _accountRepo.CreateUserAsync(newuser);
                if (result.Succeeded)
                {
                    return RedirectToAction("EmailConfirm", new { email = newuser.Email });
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("RegisterVM.Email", item.Description);
                    }
                    LoginSign.RegisterVM = newuser;
                }
            }
            return View("Login", LoginSign);
        }



        //Method That Return the View of Email Confirmation  GET
        [HttpGet]
        public async Task<IActionResult> EmailConfirm(string userid, string token, string email)
        {
            EmailConfirmModel model = new EmailConfirmModel()
            {
                email = email,
            };
            if (!string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                var user = await _accountRepo.GetUserById(userid);
                var result = await _accountRepo.EmailConfirmAsync(user, token);
                if (result.Succeeded)
                {
                    model.IsSuccess = true;
                }
            }
            return View(model);
        }



        //Method That Resend Email Confirmation Link  POST
        [HttpPost]
        public async Task<IActionResult> EmailConfirm(EmailConfirmModel model)
        {
            ApplicationUser user = await _accountRepo.GetUserByEmail(model.email);
            if (user != null)
            {
                if (user.EmailConfirmed == true)
                {
                    model.IsSuccess = true;
                    return View(model);
                }

                await _accountRepo.GenerateEmailConfirmationTokenAsync(user);
                model.IsResended = true;
            }
            return View(model);
        }



        //Method For Login View POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string? returnUrl, LoginVM model)
        {
            var LoginSign = new LoginSignVM();
            if (ModelState.IsValid)
            {
                var result = await _accountRepo.LoginAsync(model);
                if (result.Succeeded)
                {
                    var user = await _accountRepo.GetUserByEmail(model.Email);
                    if (user.EmailConfirmed == false)
                    {
                        ModelState.AddModelError("LoginVM.Email", "You did not Confirm Your Email");
                        LoginSign.LoginVM = model;
                        return View(LoginSign);
                    }
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("Home", "Home");
                }
                //cookies
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError("LoginVM.Email", "Your Email is Locked right now");
                    LoginSign.LoginVM = model;
                    return View(LoginSign);
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("LoginVM.Email", "Not Allowed");
                    LoginSign.LoginVM = model;
                    return View(LoginSign);
                }
                else
                {
                    ModelState.AddModelError("LoginVM.Email", "Wrong Credentials");
                    LoginSign.LoginVM = model;
                    return View(LoginSign);
                }
            }
                    LoginSign.LoginVM = model;
            return View(LoginSign);
        }





        // Method For Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountRepo.LogoutAsync();
            return RedirectToAction("Home", "Home");
        }





        //Method That Return the View of Enter Email to Reset The Password    ==>  GET

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View(new ForgetPasswordVM());
        }



        //Method That Return the View of Confirm Email to Reset The Password  ==>  POST
        //Make The Forget password Token And Send Email
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountRepo.GetUserByEmail(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Email", "This Email Not Existing , try a valid email");
                    return View(model);
                }

                await _accountRepo.GenerateForgetPasswordTokenAsync(user);

                model.IsSuccess = true;
                return View(model);
            }
            return View(model);
        }





        //Method That Return the View of Reset Password After Check ==> UserId , Token 
        [HttpGet]
        public IActionResult ResetPassword(string userid, string token)
        {
            if (userid != null && token != null)
            {
                ResetPasswordVM model = new ResetPasswordVM();
                token = token.Replace(' ', '+');
                if (!string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(token))
                {
                    model.Token = token;
                    model.UserId = userid;

                }
                return View(model);
            }
            return RedirectToAction("Home", "Home");
        }




        //Method That Reset Password and make New one....
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountRepo.GetUserById(model.UserId);
                model.Token = model.Token.Replace(' ', '+');
                var result = await _accountRepo.ResetPasswordConfirmAsync(user, model.Token, model.NewPassword);
                if (result.Succeeded)
                {
                    model.IsReseted = true;
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                }
            }
            return View(model);
        }




        //Return The View of Access Denied Page
        public IActionResult AccessDeny()
        {
            return View();
        }





        //  Data In User Dashboard  === Main Page   {Account}
        public async Task<IActionResult> Profile()
        {
            var model = await _accountRepo.GetAccountDataAsync();
            return Json(model);
        }




        //Data In User Dashboard  === Wish List 
        public async Task<IActionResult> UserWishList()
        {
            var model = await _accountRepo.GetWishListDataAsync();
            return Json(model);
        }




        //Data In User Dashboard  === Order
        public async Task<IActionResult> UserOrder()
        {
            var model = await _accountRepo.GetOrderAndDetailsAsync();
            return Json(model);
        }




        //Data In User Dashboard  === Account Data
        public async Task<IActionResult> AccountData()
        {
            var model = await _accountRepo.GetUserDataAsync();
            return Json(model);
        }


    }
}
