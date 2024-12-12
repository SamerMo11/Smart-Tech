using System.Diagnostics;
using E_CommerceWeb.Data;
using E_CommerceWeb.Models;
using E_CommerceWeb.Repository.Interface;
using E_CommerceWeb.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stripe.Checkout;

namespace E_CommerceWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepo _homeRepo;
        private readonly IEmailRepo _emailRepo;
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ILogger<HomeController> logger,
                              IHomeRepo homeRepo,
                              IEmailRepo emailRepo,
                              AppDbContext context,
                        UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _homeRepo = homeRepo;
            _context = context;
            _userManager = userManager;
            _emailRepo = emailRepo;
        }



        /////////////////////////////////////////////////////////////////    Pages       ///////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Return The Home Page
        public IActionResult Home()
        {
            return View();
        }



        //Return The Contact Page
        public IActionResult Contact()
        {
            return View();
        }



        //Return the Shop Page
        public IActionResult Shop()
        {
            return View();
        }


        //Return the Product Details Page
        public IActionResult ProductView()
        {
            return View();
        }



        //Return the About Page
        public IActionResult About()
        {
            return View();
        }





        //Return The Cart Page
        public IActionResult Cart()
        {
            return View();
        }



        //Return The CheckOut Page
        public async Task<IActionResult> CheckOut()
        {
            var model = await _homeRepo.GetCheckOutDataAsync();
            return View(model);
        }










        /////////////////////////////////////////////////////////////////    Logic       ///////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Contact
        //Save Info from Contact Page
        public async Task<IActionResult> SaveContact(Contact model)
        {
            if (ModelState.IsValid)
            {
                var result = await _homeRepo.AddContactAsync(model);
                if (result)
                {
                    ViewBag.Success = "Your Request has sent successfully";
                    return RedirectToAction(nameof(Contact));
                }
                ViewBag.Success = "Something wrong happen...";
                return RedirectToAction(nameof(Contact));
            }
            return View(nameof(Contact), model);
        }
        #endregion




        #region Shop

        //Return Shop Data
        public async Task<IActionResult> GetShopData(string filter)
        {
            var result = JsonConvert.DeserializeObject<ShopFilter>(filter);
            var model = await _homeRepo.GetShopDataAsync(result);
            return Json(model);
        }



        //Add Item To Favourite
        public async Task<IActionResult> AddOrRemoveItemFromFav(string obj)
        {
            if (obj == null)
                return Json(false);
            var result = JsonConvert.DeserializeObject<FavModelVM>(obj);
            await _homeRepo.AddOrRemoveFavoriteProductAsync(result.itemId, result.isfav);
            return Json(true);
        }

        #endregion




        #region Home
        // Add Item to Cart
        public async Task<IActionResult> AddItem(int productid, int quantity)
        {
            if (productid == 0)
                return NotFound();
            if (quantity == 0) quantity = 1;
            var count = await _homeRepo.AddItemAsync(productid, quantity);
            return Json(count);
        }




        // Add Offer to Cart
        public async Task<IActionResult> AddOffer(int offerid, int quantity = 1)
        {
            if (quantity < 0 || offerid == 0)
                return NotFound();
            var count = await _homeRepo.AddOfferAsync(offerid, quantity);
            return Json(count);
        }



        // Delete Item from Cart
        public async Task<IActionResult> DeleteItem(int productid)
        {
            if (productid == null)
                return NotFound();
            var count = await _homeRepo.DeleteItemAsync(productid);
            return Ok(count);
        }




        // Delete Offer from Cart      ===> return Json
        public async Task<IActionResult> DeleteOffer(int Offerid)
        {
            if (Offerid == 0)
                return NotFound();
            var count = await _homeRepo.DeleteOfferAsync(Offerid);
            return Ok(count);
        }





        //Determine the Id ==> {productIn - Offer}
        public async Task<IActionResult> DeleteAll(string type , int id)
        {
            if(type == null || id == 0) return Json(false);

            if(type == "offer")
            {
                var result = await _homeRepo.DeleteAllOfferAsync(id);
                if (result == false)
                    return Json(false);
            }
            if (type == "product")
            {
                var result = await _homeRepo.DeleteAllItemAsync(id);
                if (result == false)
                    return Json(false);
            }
            
            return Json(await _homeRepo.GetCartCountAsync());
        }







        //Update Item , Offer Quantity
        public async Task<IActionResult> UpdateItemQuantity( int id ,int quantity ,string type)
        {
            int result = 0;
            if (type == "product")
            {
                result = await _homeRepo.UpdateItemQuantityAsync(id,0,quantity);
            }
            else
            {
                result = await _homeRepo.UpdateItemQuantityAsync(0,id,quantity);
            }
            return Json(result);
        }

        //Delete All Item From Cart Details
        public async Task<IActionResult> DeleteAllItemAsync(int productid)
        {
            if (productid == 0) return NotFound();
            var result = await _homeRepo.DeleteAllItemAsync(productid);
            if (result)
                return Json(true);
            else 
                return Json(false);
        }




        //Get The Cart Number 
        public async Task<IActionResult> GetCartNumber()
        {
            return Json(await _homeRepo.GetCartCountAsync());
        }


        //Delete All offer From Cart Details
        public async Task<IActionResult> DeleteAllOfferAsync(int OfferId)
        {
            if (OfferId == 0) return NotFound();

            var result = await _homeRepo.DeleteAllOfferAsync(OfferId);
            if (result)
                return Json(true);
            else
                return Json(false);
        }



        //Get Data In The Home Page
        public async Task<IActionResult> GetHomeData()
        {
            var model = await _homeRepo.GetHomeDataAsync();
            return Json(model);
        }



        #region Search
        //Last Searches For User
        public async Task<IActionResult> GetLastSearches()
        {
            var model = await _homeRepo.GetLastSearchesAsync();
            return Json(model);
        }



        //Return Suggested Products
        public async Task<IActionResult> GetSearchProduct(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Json(null);
            var model = await _homeRepo.GetSearchProductAsync(text);
            return Json(model);
        }



        //Delete Search Result 
        public async Task<IActionResult> DeleteSearchResult(int id)
        {
            if (id == 0) return Json(false);
            var result = await _homeRepo.DeleteSearchResultAsync(id);
            if (result) return Json(true);
            return Json(null);
        }

        #endregion




        //Add Item to Favorites
        [HttpPost]
        public async Task<IActionResult> AddProductToFavorite(int productId, bool isChecked)
        {
            if (productId != 0)
            {
                await _homeRepo.AddOrRemoveFavoriteProductAsync(productId, isChecked);
                return Json(true);
            }
            return Json(false);

        }

        #endregion


        #region Cart

        //Get Cart Details ==> Data
        public async Task<IActionResult> GetCartData()
        {
            var model = await _homeRepo.GetCartDataAsync();
            if(model == null) return Json(null);
            return Json(model);
        }
        #endregion



        #region product View
        //Get Data in product View ==> {Details - define MainAttribute - Related Product}
        public async Task<IActionResult> GetProduct(int mainProductId, int? variantId)
        {
            var model = await _homeRepo.GetProductAsync(mainProductId, variantId);
            return Json(model);
        }




        //Check Validation To allow making CheckOut
        public async Task<IActionResult> CheckReviewValidation(int productId)
        {
            var model = await _homeRepo.CheckReviewValidationAsync(productId);
            return Json(model);
        }





        //Make Review
        public async Task<IActionResult> MakeReview(string Message, string RateValue, int VariantId)
        {
            var result = await _homeRepo.MakeReviewAsync(Message, int.Parse(RateValue), VariantId);
            if (!result.result) return Json(false);

            return Json(true);
        }
        #endregion



        #region CheckOut
        // Get Cites By State_ID  ====> Return Json <====
        [HttpGet]
        public async Task<IActionResult> GetCities(int stateId)
        {
            if (stateId == 0) { return Json(false); }
            var model = await _homeRepo.GetCityByStateIdAsync(stateId);
            if (model == null) { return NotFound(); }
            return Json(model);
        }




        // Send Discount Code To User
        public async Task<IActionResult> SendDiscountCode()
        {
            var userId = _homeRepo.GetUserId();
            await _emailRepo.SendDiscountCodeOnEmailAsync(userId);
            ViewBag.codesended = true;
            return RedirectToAction("CheckOut");
        }




        //Check that Code Is Valid (Check Out Page)
        public async Task<IActionResult> CheckCodeValidation(string code)
        {
            var resut = await _homeRepo.CheckCodeValidationAndGetDiscountAsync(code);
            return Json (resut);
        }




        //Check Out method 
        [HttpPost]
        public async Task<IActionResult> CheckOut(CheckOutVM model)
        {
            if (!ModelState.IsValid)
            {
                model.States = await _homeRepo.GetStatesAsync();
                return View(model);
            }
            bool coderesult = true;
            if (model.code != null)
                coderesult = await _homeRepo.CheckCodeValidationAsync(model.code);
            if (coderesult == false)
            {
                ModelState.AddModelError("code", "The Code Tou Inter Is Not Vaild .");
                model = await _homeRepo.GetCheckOutDataAsync();
                return View(model);
            }

            if (model.PaymentMethod == "Cash")
            {
                await _homeRepo.MakeOrderCashAsync(model.PaymentMethod, model.Phone, model.Address, model.City_Id, model.code != string.Empty ? false : true);
                ViewBag.success = true;
                return RedirectToAction("Home", "Home");
            }

            HttpContext.Session.SetString("Phone", model.Phone.ToString());
            HttpContext.Session.SetString("PaymentMethod", model.PaymentMethod.ToString());
            HttpContext.Session.SetString("Address", model.Address.ToString());
            HttpContext.Session.SetString("CityId", model.City_Id.ToString());
            HttpContext.Session.SetString("Code", model.code == null ? "null" : model.code.ToString());


            //var options = await _homeRepo.CheckOutAsync(model.code);
            var userId = _homeRepo.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var Code = await _context.DiscountCodes.FirstOrDefaultAsync(x => x.Code == model.code && !x.IsUsed);

            var cart = await _homeRepo.GetCartDetailsAsync();
            var Domain = "https://localhost:7271/";

            var options = new SessionCreateOptions()
            {
                SuccessUrl = Domain + "Home/ConfirmCheckOut",
                CancelUrl = Domain + "Home/CheckOut",
                Mode = "payment",
                LineItems = new List<SessionLineItemOptions>()
            };

            foreach (var product in cart.cartDetailsItemVMs)
            {
                var LineItems = new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "Egp",
                        UnitAmount = Code == null ? (long)(CalcPrice(product.Price, product.Discount) * 100) : (long)(CalcPrice(product.Price, product.Discount) * 100 * (1 - ((double)Code.DiscountPercentage / 100))),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.Product.Name,
                            Description = product.Product.Description,
                        }
                    },
                    Quantity = product.QuantityInCart
                };
                options.LineItems.Add(LineItems);
            }


            foreach (var product in cart.cartDetailsOfferVMs)
            {
                var LineItems = new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "Egp",
                        UnitAmount = Code == null ? ((long)product.Price * 100) : (long)(product.Price * 100 * (1 - ((double)Code.DiscountPercentage / 100))),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.MainName,
                            Description = product.Discription,
                        }
                    },
                    Quantity = product.QuantityInCart
                };
                options.LineItems.Add(LineItems);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            HttpContext.Session.SetString("SessionID", session.Id);
            return Redirect(session.Url);
        }

        double CalcPrice(double price, int dis)
        {
            var discount = (float)1 - ((float)dis / (float)100);
            var result = price * discount;
            return Math.Round(result, 2);
        }



        //Confirm Check Out method 
        public async Task<IActionResult> ConfirmCheckOut()
        {
            var service = new SessionService();
            Session session = service.Get(HttpContext.Session.GetString("SessionID"));
            var phone = HttpContext.Session.GetString("Phone");
            var PaymentMethod = HttpContext.Session.GetString("PaymentMethod");
            var address = HttpContext.Session.GetString("Address");
            var cityId = int.Parse(HttpContext.Session.GetString("CityId"));
            var Code = HttpContext.Session.GetString("Code");
            if (session.PaymentStatus == "paid")
            {
                // showen the code is used or not
                if (Code != "null")
                    await _homeRepo.GetDiscountCodeByCodeAsync(Code);
                await _homeRepo.MakeOrderOnLineAsync(PaymentMethod, phone, address, cityId, session.Id, session.PaymentIntentId, Code != "null" ? true : false);
                Debug.WriteLine("MakeOrderOnLineAsync done");
                ViewBag.success = true;
                return RedirectToAction("Home", "Home");
            }
            else
            {
                ViewBag.failed = true;
                return RedirectToAction("checkOut");
            }
        }
        #endregion



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
