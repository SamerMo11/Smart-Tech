using E_CommerceWeb.Data;
using E_CommerceWeb.Models;
using E_CommerceWeb.Repository.Interface;
using E_CommerceWeb.ViewModels.Email;
using E_CommerceWeb.ViewModels.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;

namespace E_CommerceWeb.Repository
{
    public class HomeRepo : IHomeRepo
    {

        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _http;

        public HomeRepo(AppDbContext context,
                        UserManager<ApplicationUser> userManager,
                        IHttpContextAccessor http)
        {
            _context = context;
            _userManager = userManager;
            _http = http;
        }

        //Get User Id
        public string GetUserId()
        {
            var user = _http.HttpContext.User;
            var UserId = _userManager.GetUserId(user);
            return UserId;
        }


        //Save Contact Fields in DB
        public async Task<bool> AddContactAsync(Contact model)
        {
            try
            {
                var item = new Contact
                {
                    Name = model.Name,
                    Company = model.Company ?? null,
                    Phone = model.Phone,
                    Email = model.Email,
                    Subject = model.Subject ?? null,
                    Message = model.Message,
                    UserId = GetUserId() ?? null,
                };
                await _context.Contacts.AddAsync(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }






        //Method for Get ShoppingCart of specific User through User_Id
        private async Task<ShoppingCart> GetShoppingCartByUserId(string UserId)
        {
            var cart = await _context.ShoppingCarts.Where(x => x.User_Id == UserId).FirstOrDefaultAsync();
            return cart;
        }







        //Method for Count of items in the cart ===> according to Specific User
        public async Task<int> GetCartCountAsync()
        {
            var userid = GetUserId();
            var cart = from c in _context.ShoppingCarts
                       join d in _context.CartDetails
                       on c.Id equals d.ShoppingCart_Id
                       where c.User_Id == userid
                       select new { d.Quantity };
            return cart.Count();
        }






        //Method for Adding Item to Cart
        public async Task<int> AddItemAsync(int productvariantid, int quantity)
        {
            using var transiant = _context.Database.BeginTransaction();
            try
            {
                var productIn = await _context.Variants.FindAsync(productvariantid);
                var userid = GetUserId();
                if (userid == null)
                    throw new Exception("User Not Exist ....");
                var Cart = await GetShoppingCartByUserId(userid);
                if (Cart == null)
                {
                    Cart = new ShoppingCart
                    {
                        User_Id = userid
                    };
                    await _context.ShoppingCarts.AddAsync(Cart);
                    await _context.SaveChangesAsync();
                }

                var CartDetails = await _context.CartDetails.Where(x => x.ShoppingCart_Id == Cart.Id && x.ProductIn_Id == productvariantid).FirstOrDefaultAsync();
                if (CartDetails == null)
                {
                    CartDetails = new CartDetails
                    {
                        ProductIn_Id = productvariantid,
                        ShoppingCart_Id = Cart.Id,
                        Quantity = quantity,
                        UnitPrice = productIn.Discount == 0 ? productIn.Price : Math.Round(productIn.Price * (1 - ((double)productIn.Discount / 100)), 2)
                    };
                    await _context.CartDetails.AddAsync(CartDetails);
                }
                else
                {
                    if (productIn.Quantity > CartDetails.Quantity)
                        CartDetails.Quantity += quantity;
                }
                await _context.SaveChangesAsync();
                transiant.Commit();
            }
            catch (Exception ex) { }
            return await GetCartCountAsync();
        }







        //Method for Adding Offer to Cart
        public async Task<int> AddOfferAsync(int Offerid, int quantity)
        {
            using var transiant = _context.Database.BeginTransaction();
            try
            {
                var Offer = await _context.Offers.FindAsync(Offerid);
                var userid = GetUserId();
                if (userid == null)
                    throw new Exception("User Not Exist ....");
                var Cart = await GetShoppingCartByUserId(userid);
                if (Cart == null)
                {
                    Cart = new ShoppingCart
                    {
                        User_Id = userid
                    };
                    await _context.ShoppingCarts.AddAsync(Cart);
                    await _context.SaveChangesAsync();
                }

                var CartDetails = await _context.CartDetails.Where(x => x.ShoppingCart_Id == Cart.Id && x.OfferId == Offerid).FirstOrDefaultAsync();
                if (CartDetails == null)
                {
                    CartDetails = new CartDetails
                    {
                        OfferId = Offerid,
                        ShoppingCart_Id = Cart.Id,
                        Quantity = quantity,
                        UnitPrice = Offer.Price,
                    };
                    await _context.CartDetails.AddAsync(CartDetails);
                }
                else
                {
                    CartDetails.Quantity += quantity;
                }
                await _context.SaveChangesAsync();
                transiant.Commit();
            }
            catch (Exception ex) { }
            return await GetCartCountAsync();
        }







        //Method for Deleting Item to Cart
        public async Task<int> DeleteItemAsync(int productvariantid)
        {
            using var transiant = _context.Database.BeginTransaction();
            try
            {
                var userid = GetUserId();
                if (userid == null)
                    throw new Exception("User Not Exist ....");
                var Cart = await GetShoppingCartByUserId(userid);
                if (Cart == null)
                {
                    throw new Exception("You Have no cart to delete");
                }

                var CartDetails = await _context.CartDetails.Where(x => x.ShoppingCart_Id == Cart.Id && x.ProductIn_Id == productvariantid).FirstOrDefaultAsync();
                if (CartDetails == null)
                {
                    throw new Exception("Cart Already empty");
                }
                else if (CartDetails.Quantity == 1)
                {
                    _context.CartDetails.Remove(CartDetails);
                }
                else
                {
                    CartDetails.Quantity -= 1;
                }
                await _context.SaveChangesAsync();
                transiant.Commit();
            }
            catch (Exception ex) { }
            return await GetCartCountAsync();
        }






        //Method for Deleting Offer form Cart
        public async Task<int> DeleteOfferAsync(int Offerid)
        {
            using var transiant = _context.Database.BeginTransaction();
            try
            {
                var userid = GetUserId();
                if (userid == null)
                    throw new Exception("User Not Exist ....");
                var Cart = await GetShoppingCartByUserId(userid);
                if (Cart == null)
                {
                    throw new Exception("You Have no cart to delete");
                }

                var CartDetails = await _context.CartDetails.Where(x => x.ShoppingCart_Id == Cart.Id && x.OfferId == Offerid).FirstOrDefaultAsync();
                if (CartDetails == null)
                {
                    throw new Exception("Cart Already empty");
                }
                else if (CartDetails.Quantity == 1)
                {
                    _context.CartDetails.Remove(CartDetails);
                }
                else
                {
                    CartDetails.Quantity -= 1;
                }
                await _context.SaveChangesAsync();
                transiant.Commit();
            }
            catch (Exception ex) { }
            return await GetCartCountAsync();
        }







        //Delete All Item From Cart Details
        public async Task<bool> DeleteAllItemAsync(int ProductId)
        {
            var model = await _context.CartDetails.Where(x => x.ProductIn_Id == ProductId).ToListAsync();
            if(model == null)
                return false;
            _context.CartDetails.RemoveRange(model);
            await _context.SaveChangesAsync();
            return true;
        }






        //Delete All Offer From Cart Details
        public async Task<bool> DeleteAllOfferAsync(int OfferId)
        {
            var model = await _context.CartDetails.Where(x => x.OfferId == OfferId).ToListAsync();
            if (model == null)
                return false;
            _context.CartDetails.RemoveRange(model);
            await _context.SaveChangesAsync();
            return true;
        }



        


        //Get Data in product View ==> {Details - define MainAttribute - Related Product - Reviews}
        public async Task<dynamic> GetProductAsync(int mainProductId, int? variantId)
        {
            var Variant = new ProductVariant();
            List<string> Images = new List<string>();

            if (variantId != 0)
                Variant = await _context.Variants.FirstOrDefaultAsync(x => x.Id == variantId);
            else
                Variant = await _context.Variants.FirstOrDefaultAsync();

            var ImagesList = await _context.VariantImages.Where(x => x.ProductVariant_Id == Variant.Id).Select(a => a.ImagePath).ToListAsync();
            Images.AddRange(ImagesList);


            var MainProduct = await _context.Products.FindAsync(mainProductId);
            var Category = await _context.categories.Select(a => new
            {
                Id = a.Id,
                Name = a.Name
            }).FirstOrDefaultAsync(x => x.Id == MainProduct.Category_Id);


            var Variants = await _context.Variants.AsNoTracking().Where(x => x.Product_Id == mainProductId).ToListAsync();
            var Specific = await _context.ProductAttributes.Include(x => x.CategoryAttribute).Where(x => x.Product_Id == mainProductId).ToListAsync();




            Dictionary<string, List<combinedAttributes>> attributeValues = new Dictionary<string, List<combinedAttributes>>();

            var mainAttributesByCategory = new Dictionary<string, List<string>>
            {
                { "Laptop", new List<string> { "Storage", "RAM" } },
                { "Phone", new List<string> { "RAM", "Processor", "Battery" } },

            };


            var Products = await _context.Products.Include(x => x.category).Where(x => x.Name.ToLower().Contains(MainProduct.Name.ToLower())).ToListAsync();
            foreach (var item in Products)
            {
                item.ProductAttributes = await _context.ProductAttributes.Include(x => x.CategoryAttribute).Where(x => x.Product_Id == item.Id).ToListAsync();


                var mainAttributes = mainAttributesByCategory.ContainsKey(item.category.Name)
                                    ? mainAttributesByCategory[item.category.Name]
                                    : new List<string>(); // Default to empty if category not in mapping

                foreach (var attribute in item.ProductAttributes)
                {
                    var attributeName = attribute.CategoryAttribute.Name;
                    // Only process attributes that are in the main attributes list
                    if (mainAttributes.Contains(attributeName))
                    {
                        if (!attributeValues.ContainsKey(attributeName))
                        {
                            attributeValues[attributeName] = new List<combinedAttributes>();
                        }
                        attributeValues[attributeName].Add(new combinedAttributes
                        {
                            id = attribute.Product_Id,
                            Value = attribute.Value
                        });
                    }
                }
            }


            List<dynamic> RelatedVariants = new List<dynamic>();
            var RelatedMainProduct = await _context.Products.Where(x => x.Category_Id == Category.Id).ToListAsync();
            foreach (var item in RelatedMainProduct)
            {
                var data = await _context.Variants.Where(x => x.Product_Id == item.Id).Select(a => new 
                {
                    MainId = a.Product_Id,
                    Id = a.Id,
                    Name = item.Name,
                    Price = a.Price,
                    Image = a.ImagePath,
                    TotalRating = a.TotalRating,
                    UserCountRating = a.TotalRating,
                    IsFavorite = a.IsFavorite,
                }).ToListAsync();
                RelatedVariants.AddRange(data);
            }




            var reviews = await _context.Reviews.AsSplitQuery().AsNoTrackingWithIdentityResolution().Include(x => x.ApplicationUser)
                                         .Where(X => X.productVariantId == variantId).Select(a => new 
                                         {
                                             Date = a.CreatedAt.ToString("d"),
                                             Image = a.ApplicationUser.ImagePath,
                                             Message = a.Message,
                                             RateValue = a.RateValue,
                                             UserName = a.ApplicationUser.FirstName + a.ApplicationUser.LastName
                                         }).ToListAsync();

            var model = new
            {
                Main_Id = MainProduct.Id,
                Name = MainProduct.Name,
                Description = MainProduct.Description,
                Category = Category,
                Details = new
                {
                    Id = Variant.Id,
                    Color = Variant.Color,
                    Price = Variant.Price,
                    Discount = Variant.Discount,
                    IsFavourite = Variant.IsFavorite,
                    Quantity = Variant.Quantity,
                    TotalRating = Variant.TotalRating,
                    UserCountRating = Variant.UserCountRating
                },
                SendedId = variantId,
                Images = Images,
                Colors = Variants.Select(a => new
                {
                    Id = a.Id,
                    Color = a.Color,
                }).ToList(),
                Specifications = Specific.Select(a => new
                {
                    Key = a.CategoryAttribute.Name,
                    Value = a.Value
                }),
                CombinedAttributes = attributeValues,
                RelatedProducts = RelatedVariants.OrderBy(x => Guid.NewGuid()),
                Review = reviews
            };
            return model;
        }






        //Get All Cart Details Data
        public async Task<ShoppingCartVM> GetCartDetailsAsync()
        {
            var UserId = GetUserId();
            ShoppingCartVM model = new();
            if (UserId != null)
            {
                var Cart = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.User_Id == UserId);
                var CartDetails = await _context.CartDetails.Where(x => x.ShoppingCart_Id == Cart.Id).ToListAsync();

                List<CartDetailsItemVM> CartDetailsVM = new();
                List<CartDetailsOfferVM> offerVMs = new();
                foreach (var item in CartDetails)
                {
                    var ProductIn = await _context.Variants.FirstOrDefaultAsync(x => x.Id == item.ProductIn_Id);
                    var Offer = await _context.Offers.AsSplitQuery().Include(x => x.OfferProducts).FirstOrDefaultAsync(x => x.Id == item.OfferId);
                    if (ProductIn != null)
                    {
                        var obj = new CartDetailsItemVM
                        {
                            CartDetail_Id = item.Id,
                            ProductVAriant_Id = ProductIn.Id,
                            Color = ProductIn.Color,
                            ImagePath = ProductIn.ImagePath,
                            Price = ProductIn.Price,
                            Quantity = ProductIn.Quantity,
                            QuantityInCart = item.Quantity,
                            Product = _context.Products.Include(x => x.category).FirstOrDefault(x => x.Id == ProductIn.Product_Id),
                            Discount = ProductIn.Discount
                        };
                        CartDetailsVM.Add(obj);
                    }
                    if (Offer != null)
                    {
                        var OfferProducts = await _context.OfferProducts.AsSplitQuery().Include(x => x.ProductVariant).ThenInclude(pv => pv.Product).Where(x => x.OfferId == Offer.Id).ToListAsync();
                        var Names = new List<string>();
                        var Images = new List<string>();
                        var obj = new CartDetailsOfferVM
                        {
                            CartDetail_Id = item.Id,
                            OfferId = Offer.Id,
                            Discription = Offer.Description,
                            Price = Offer.Price,
                            QuantityInCart = item.Quantity,
                            MainName = Offer.Name
                        };
                        foreach (var OfferProduct in OfferProducts)
                        {
                            Names.Add(OfferProduct.ProductVariant.Product.Name);
                            Images.Add(OfferProduct.ProductVariant.ImagePath);
                        }
                        obj.Names = Names;
                        obj.ImagePaths = Images;
                        offerVMs.Add(obj);
                    }
                }

                model.cartDetailsItemVMs = CartDetailsVM;
                model.cartDetailsOfferVMs = offerVMs;
                model.Cart_ID = Cart.Id;
            }
            return model;
        }






        // View And Keep Data in Check Out Page And Make The Whole Model 
        public async Task<CheckOutVM> GetCheckOutDataAsync()
        {
            var userId = GetUserId();
            CheckOutVM model = new CheckOutVM
            {
                ShoppingCart = await GetCartDetailsAsync(),
                States = await _context.States.ToListAsync(),
                UESR = await _context.Users.Include(x => x.DiscountCode).FirstOrDefaultAsync(x => x.Id == userId)
            };
            return model;
        }






        // Get Cites By State_ID 
        public async Task<IEnumerable<City>> GetCityByStateIdAsync(int stateid)
        {
            var ciry = await _context.Cities.Where(x => x.State_Id == stateid).ToListAsync();
            return ciry;
        }

        // return States
        public async Task<List<state>> GetStatesAsync()
        {
            return await _context.States.ToListAsync();
        }





        //Check that Code Is Valid (Check Out Page)
        public async Task<decimal> CheckCodeValidationAndGetDiscountAsync(string code)
        {
            var coderow = await _context.DiscountCodes.FirstOrDefaultAsync(x => x.Code == code);

            if (coderow == null) return 0;
            var result = coderow.IsUsed;
            if (result == false)
            {
                return coderow.DiscountPercentage.Value;
            }
            return 0;

        }


        // Check Existing Code
        public async Task<bool> CheckCodeValidationAsync(string code)
        {
            if (code == null)
                return true;
            var DiscountCode = await _context.DiscountCodes.FirstOrDefaultAsync(x => x.Code == code && !x.IsUsed);
            if (DiscountCode != null)
            {
                return true;
            }
            return false;
        }





        // Update the amount of Product according to Orders Operation 
        private async Task UpdateProductQuantity(int productvariantId, int quantity)
        {
            var productIn = await _context.Variants.FindAsync(productvariantId);
            if (productIn == null) throw new Exception("Product is not exist");
            productIn.Quantity = productIn.Quantity - quantity;
            _context.Variants.Update(productIn);
        }



        //make Order and It's Details and clear Cart Details {Cash}
        public async Task MakeOrderCashAsync(string PaymentMethod, string phone, string address, int cityId, bool HasDiscount)
        {
            using var transation = _context.Database.BeginTransaction();
            try
            {
                var userId = GetUserId();
                if (userId == null) { throw new Exception("User Not Login"); }
                var cart = await GetCartDetailsAsync();
                if (cart == null) { throw new Exception("User has no shoppingcart"); }

                var carddetails = await _context.CartDetails.Where(x => x.ShoppingCart_Id == cart.Cart_ID).ToListAsync();
                if (carddetails == null) { throw new Exception("User has no shoppingcart Details"); }

                var order = new Order
                {
                    //Code here   <================================
                    Phone = phone,
                    Address = address,
                    City_Id = cityId,
                    Status_id = 1,
                    User_Id = userId,
                    paymentMethod = PaymentMethod,
                    paymentStatus = "Pending",
                    CreatedDate = DateTime.Now,
                    HasDiscount = HasDiscount
                };
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                foreach (var item in carddetails)
                {
                    if (item.ProductIn_Id != null)
                    {
                        var orderdetails = new OrderDetails()
                        {
                            Order_Id = order.Id,
                            ProductIn_Id = item.ProductIn_Id.Value,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice
                        };
                        await _context.OrderDetails.AddAsync(orderdetails);
                        await UpdateProductQuantity(item.ProductIn_Id.Value, item.Quantity);
                    }
                    else
                    {
                        var orderdetails = new OrderDetails()
                        {
                            Order_Id = order.Id,
                            OfferId = item.OfferId.Value,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice
                        };
                        await _context.OrderDetails.AddAsync(orderdetails);
                    }
                    await _context.SaveChangesAsync();
                }
                _context.CartDetails.RemoveRange(carddetails);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex) { }
            transation.Commit();
        }
        //make Order and It's Details and clear Cart Details {OnLine}
        public async Task MakeOrderOnLineAsync(string PaymentMethod, string phone, string address, int cityId, string StripeSessionId, string StripePaymentIntentId, bool HasDiscount)
        {
            using var transation = _context.Database.BeginTransaction();
            try
            {
                var userId = GetUserId();
                if (userId == null) { throw new Exception("User Not Login"); }
                var cart = await GetCartDetailsAsync();
                if (cart == null) { throw new Exception("User has no shoppingcart"); }

                var carddetails = await _context.CartDetails.Where(x => x.ShoppingCart_Id == cart.Cart_ID).ToListAsync();
                if (carddetails == null) { throw new Exception("User has no shoppingcart Details"); }

                var order = new Order
                {
                    //Code here   <================================
                    Phone = phone,
                    Address = address,
                    City_Id = cityId,
                    Status_id = 1,
                    User_Id = userId,
                    paymentMethod = PaymentMethod,
                    paymentStatus = "Approved",
                    CreatedDate = DateTime.Now,
                    StripeSessionId = StripeSessionId,
                    StripePaymentIntentId = StripePaymentIntentId,
                    HasDiscount = HasDiscount
                };
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                foreach (var item in carddetails)
                {
                    var orderdetails = new OrderDetails()
                    {
                        Order_Id = order.Id,
                        ProductIn_Id = item.ProductIn_Id.Value,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    await _context.OrderDetails.AddAsync(orderdetails);
                    await UpdateProductQuantity(item.ProductIn_Id.Value, item.Quantity);
                    await _context.SaveChangesAsync();
                }
                _context.CartDetails.RemoveRange(carddetails);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex) { }
            transation.Commit();
        }





        double CalcPrice(double price, int dis)
        {
            var discount = (float)1 - ((float)dis / (float)100);
            var result = price * discount;
            return Math.Round(result, 2);
        }

        //make checkout and move to stripe web
        public async Task<SessionCreateOptions> CheckOutAsync(string code)
        {
            var userId = GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var Code = await _context.DiscountCodes.FirstOrDefaultAsync(x => x.Code == code && !x.IsUsed);

            var cart = await GetCartDetailsAsync();
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

            
            return options;
        }









        // Get DiscountCode By Name and update IsUsed To ===> TRUE
        public async Task GetDiscountCodeByCodeAsync(string code)
        {
            var CodeItem = await _context.DiscountCodes.SingleAsync(x => x.Code == code);
            if (CodeItem != null && !CodeItem.IsUsed)
            {
                CodeItem.IsUsed = true;
                _context.DiscountCodes.Update(CodeItem);
                await _context.SaveChangesAsync();
            }
        }

    }
}
