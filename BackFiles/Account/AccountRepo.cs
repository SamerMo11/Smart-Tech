using E_CommerceWeb.Data;
using E_CommerceWeb.Models;
using E_CommerceWeb.Repository.Interface;
using E_CommerceWeb.ViewModels.Account;
using E_CommerceWeb.ViewModels.Account.DashData;
using E_CommerceWeb.ViewModels.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace E_CommerceWeb.Repository
{
    public class AccountRepo : IAccountRepo
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailRepo _emailRepo;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public AccountRepo(UserManager<ApplicationUser> userManager,
                           IEmailRepo emailRepo,
                           SignInManager<ApplicationUser> signInManager,
                           AppDbContext context,
                           IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _emailRepo = emailRepo;
            _signInManager = signInManager;
            _context = context;
            _httpContext = httpContext;
        }



        //Retrun UserID From Cookies
        private string GetUserId()
        {
            var principal = _httpContext.HttpContext.User;
            var userId = _userManager.GetUserId(principal);
            return userId;
        }


        //Return User By Id
        public async Task<ApplicationUser> GetUserById(string userid)
        {
            return await _userManager.FindByIdAsync(userid);
        }

        //Return User By Email
        public async Task<ApplicationUser> GetUserByEmail(string Email)
        {
            return await _userManager.FindByEmailAsync(Email);
        }






        //Create user 
        public async Task<IdentityResult> CreateUserAsync(RegisterVM model)
        {
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.Number,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await GenerateEmailConfirmationTokenAsync(user);
            }
            return result;
        }






        //Make Email Confirmation Token
        public async Task GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (token != null)
            {
                await SendEmailConfirmation(user, token);
            }
        }





        //private method to Send EmailConfirmation
        private async Task SendEmailConfirmation(ApplicationUser user, string token)
        {
            var Domain = "https://localhost:7271/";
            var EmailConfirmMethodPath = "Account/EmailConfirm?userid={0}&token={1}";

            UserEmailOptions emailOptions = new UserEmailOptions
            {
                RecieveEmails = new List<string> { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("{{username}}",user.FirstName+" "+user.LastName),
                    new KeyValuePair<string, string>("{{link}}",string.Format(Domain+EmailConfirmMethodPath,user.Id,token))
                }
            };
            await _emailRepo.SendEmailConfirmation(emailOptions);

        }





        //Email Confirm Method
        public async Task<IdentityResult> EmailConfirmAsync(ApplicationUser user, string token)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result;
        }





        //Accept login 
        public async Task<SignInResult> LoginAsync(LoginVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return SignInResult.NotAllowed;
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            return result;
        }




        // Method for Logout 
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }




        //make token for Forget Password 
        public async Task GenerateForgetPasswordTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (token != null)
            {
                await SendForgetPasswordConfirmation(user, token);
            }
        }



        //private method to handle the send Forget password Email to user
        private async Task SendForgetPasswordConfirmation(ApplicationUser user, string token)
        {
            var Domain = "https://localhost:7271/";
            var EmailConfirmMethodPath = "Account/ResetPassword?userid={0}&token={1}";

            UserEmailOptions emailOptions = new UserEmailOptions
            {
                RecieveEmails = new List<string> { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("{{username}}",user.FirstName+" "+user.LastName),
                    new KeyValuePair<string, string>("{{link}}",string.Format(Domain+EmailConfirmMethodPath,user.Id,token))
                }
            };
            await _emailRepo.SendForgetPasswordConfirmation(emailOptions);

        }




        //Make Reset Password Method
        public async Task<IdentityResult> ResetPasswordConfirmAsync(ApplicationUser user, string token, string Password)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, Password);
            return result;
        }





        //Get User Dashboard Data ==> Main Page {First Page}
        public async Task<AccountVM> GetAccountDataAsync()
        {
            var userId = GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var orders = await _context.Orders.AsSplitQuery().AsNoTrackingWithIdentityResolution().Include(x => x.OrderDetails).Include(z => z.status).OrderByDescending(x => x.CreatedDate).Where(x => x.User_Id == userId).Take(2).Select(a => new AccountOrderVM
            {
                CreatedDate = a.CreatedDate,
                OrderId = a.Id,
                paymentStatus = a.paymentStatus,
                ProductCounts = a.OrderDetails.Count(),
                StatusId = a.status.Id,
                StatusName = a.status.Name,
                accountOrderDetails = _context.OrderDetails.AsSplitQuery().Include(x => x.ProductVariant).ThenInclude(pv => pv.Product).ThenInclude(p => p.category).Where(x => x.Order_Id == a.Id).Select(a => new AccountOrderDetailsVM
                {
                    ProductName = a.ProductVariant.Product.Name,
                    Category = a.ProductVariant.Product.category.Name,
                    ImagePath = a.ProductVariant.ImagePath,
                    Price = a.UnitPrice,
                    Quantity = a.Quantity
                }).ToList()
            }).ToListAsync();
            var UserCart = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.User_Id == userId);
            var CartNumber = UserCart == null ? 0 : await _context.CartDetails.Where(x => x.ShoppingCart_Id == UserCart.Id).CountAsync();
            var WithList = await _context.Favourites.Where(x => x.User_Id == userId).ToListAsync();
            var WithListNum = WithList == null ? 0 : await _context.Favourites.Where(x => x.User_Id == userId).CountAsync();
            AccountVM model = new AccountVM
            {
                UserName = user.FirstName + " " + user.LastName,
                Email = user.Email,
                CartNumber = CartNumber,
                WishNumber = WithListNum,
                accountOrders = orders,
            };
            return model;
        }





        //Get User Dashboard Data ==> WishList Page {Third Page}
        public async Task<dynamic> GetWishListDataAsync()
        {
            var userId = GetUserId();
            var all = await _context.Favourites.Where(x => x.User_Id == userId)
                                               .Join(_context.Variants, x => x.ProductIn_Id, y => y.Id, (x, y) => new 
                                               {
                                                   VariantId = y.Id,
                                                   Color = y.Color,
                                                   Image = y.ImagePath,
                                                   Discount = y.Discount,
                                                   UserCountRating = y.UserCountRating,
                                                   Quantity = y.Quantity,
                                                   TotalRating = y.TotalRating,
                                                   IsFavorite = true,
                                                   Price = y.Price,
                                                   ProductName = _context.Products.FirstOrDefault(z => z.Id == x.ProductVariant.Product.Id).Name,
                                                   ProductDescription = _context.Products.FirstOrDefault(z => z.Id == x.ProductVariant.Product.Id).Description,
                                                   Category = _context.Products.AsSplitQuery().Include(z => z.category).FirstOrDefault(z => z.Id == x.ProductVariant.Product.Id).category.Name
                                               }).ToListAsync();
            return all;
        }





        //Get User Dashboard Data ==> Order Page {Second Page}
        public async Task<List<AccountOrderVM>> GetOrderAndDetailsAsync()
        {
            var userId = GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var orders = await _context.Orders.AsSplitQuery().AsNoTrackingWithIdentityResolution().Include(x => x.OrderDetails).Include(z => z.status).OrderByDescending(x => x.CreatedDate).Where(x => x.User_Id == userId).Select(a => new AccountOrderVM
            {
                CreatedDate = a.CreatedDate,
                OrderId = a.Id,
                paymentStatus = a.paymentStatus,
                ProductCounts = a.OrderDetails.Count(),
                StatusId = a.status.Id,
                StatusName = a.status.Name,
                accountOrderDetails = _context.OrderDetails.AsSplitQuery().Include(x => x.ProductVariant).ThenInclude(pv => pv.Product).ThenInclude(p => p.category).Where(x => x.Order_Id == a.Id).Select(a => new AccountOrderDetailsVM
                {
                    ProductName = a.ProductVariant.Product.Name,
                    Category = a.ProductVariant.Product.category.Name,
                    ImagePath = a.ProductVariant.ImagePath,
                    Price = a.UnitPrice,
                    Quantity = a.Quantity
                }).ToList()
            }).ToListAsync();
           
            return orders;
        }



        //Get User Dashboard Data ==> User Data {Fourth Page}
        public async Task<dynamic> GetUserDataAsync()
        {
            var UserId = GetUserId();
            var user = await _userManager.FindByIdAsync(UserId);
            var Model = new 
            {
                UserId = UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Phone = user.PhoneNumber,
                Image = user.ImagePath == null ? null : user.ImagePath
            };

            return Model;
        }

    }
}
