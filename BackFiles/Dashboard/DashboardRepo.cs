using E_CommerceWeb.Data;
using E_CommerceWeb.Models;
using E_CommerceWeb.Repository.Interface;
using E_CommerceWeb.ViewModels.Dashboard.Category;
using E_CommerceWeb.ViewModels.Dashboard.MainPage;
using E_CommerceWeb.ViewModels.Dashboard.Product;
using E_CommerceWeb.ViewModels.Dashboard.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_CommerceWeb.Repository
{
    public class DashboardRepo : IDashboardRepo
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHost;

        public DashboardRepo(AppDbContext context,
                             IWebHostEnvironment webHost) 
        {
            _context = context;
            _webHost = webHost;
        }



        #region Data In Main Page
        //Combination of Data In The Main Page in Dashboard
        public async Task<MainPageVM> MainPageDataAsync()
        {
            var model = new MainPageVM();
            model.TotalSales = await totalSalesAsync();
            model.TotalRefundOrders = await totalRefundOrdersAsync();
            model.TotalApprovedOrders = await totalApprovedOrdersAsync();
            model.OrderAnalytics = await OrderCompleteAndReturnChartVM();
            model.OrdersByCategory = await DonutChartAsync();
            model.productList = await ProductListDetailsVMs();
            model.MostSelling = await MostSaleProductAsync();
            return model;
        }

        //Total Sales 
        public async Task<dynamic> totalSalesAsync()
        {
            var totalsales = await _context.OrderDetails.Include(x => x.Order).Where(x => x.Order.CreatedDate.Date <= DateTime.Today.Date && x.Order.CreatedDate.Date >= DateTime.Today.Date.AddDays(-7))
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var days = new List<double>();

            for (DateTime i = DateTime.Today.Date; i > DateTime.Today.Date.AddDays(-7); i=i.AddDays(-1))
            {
                var go = await _context.OrderDetails.Include(x => x.Order).Where(x => x.Order.CreatedDate.Date == i).SumAsync(x => x.Quantity * x.UnitPrice);
                days.Add(go);
            }

            var model = new
            {
                value = totalsales,
                days = days
            };
            return model;
        }

        //Total Refund Orders
        public async Task<dynamic> totalRefundOrdersAsync()
        {
            var totalRefundOrders = await _context.Orders.Where(x => x.CreatedDate.Date <= DateTime.Today.Date && x.CreatedDate.Date >= DateTime.Today.Date.AddDays(-7) && x.paymentStatus == "Refund").CountAsync();


            var days = new List<double>();

            for (DateTime i = DateTime.Today.Date; i > DateTime.Today.Date.AddDays(-7); i = i.AddDays(-1))
            {
                var go = await _context.Orders.Where(x => x.CreatedDate.Date == i && x.paymentStatus == "Refund").CountAsync();
                days.Add(go);
            }

            var model = new
            {
                value = totalRefundOrders,
                days = days
            };
            return model;
        }

        //Total Approved Orders
        public async Task<dynamic> totalApprovedOrdersAsync()
        {
            var totalApprovedOrders = await _context.Orders.Where(x => x.CreatedDate.Date <= DateTime.Today.Date && x.CreatedDate.Date >= DateTime.Today.Date.AddDays(-7) && x.paymentStatus != "Refund").CountAsync();
            var days = new List<double>();

            for (DateTime i = DateTime.Today.Date; i > DateTime.Today.Date.AddDays(-7); i = i.AddDays(-1))
            {
                var go = await _context.Orders.Where(x => x.CreatedDate.Date == i && x.paymentStatus != "Refund").CountAsync();
                days.Add(go);
            }

            var model = new
            {
                value = totalApprovedOrders,
                days = days
            };
            return model;
        }

        //Most Saled Product 
        public async Task<List<MostSaledVM>> MostSaleProductAsync()
        {
            var orderDetails = await _context.OrderDetails.Include(x => x.Order)
                                          .Where(x => x.Order.CreatedDate.Date <= DateTime.Today.Date && x.Order.CreatedDate.Date >= DateTime.Today.Date.AddDays(-7))
                                          .GroupBy(x => x.ProductIn_Id ?? 0).Select(a => new MostSaledVM
                                          {
                                              VariantId = a.Key,
                                              SaledQuantity = a.Sum(x => x.Quantity),
                                          }).ToListAsync();

            var wrong1 = orderDetails.FirstOrDefault(x => x.VariantId == 0);
            if (wrong1 != null)
                orderDetails.Remove(wrong1);

            var Offers = await _context.OrderDetails.Include(x => x.Order)
                                          .Where(x => x.Order.CreatedDate.Date <= DateTime.Today.Date && x.Order.CreatedDate.Date >= DateTime.Today.Date.AddDays(-7))
                                          .GroupBy(x => x.OfferId ?? 0).Select(a => new
                                          {
                                              OfferId = a.Key,
                                              OfferQuantity = a.Sum(x => x.Quantity)
                                          }).ToListAsync();

            var wrong2 = Offers.FirstOrDefault(x => x.OfferId == 0);
            if (wrong2 != null)
                Offers.Remove(wrong2);

            foreach (var item in Offers)
            {
                var Offer = await _context.Offers.AsSplitQuery().Include(x => x.OfferProducts).FirstOrDefaultAsync(x => x.Id == item.OfferId);
                foreach (var offerproduct in Offer.OfferProducts)
                {
                    MostSaledVM obj = new MostSaledVM
                    {
                        VariantId = offerproduct.ProductVariantId,
                        SaledQuantity = item.OfferQuantity * offerproduct.Quantity
                    };
                    orderDetails.Add(obj);
                }
            }

            orderDetails = orderDetails.GroupBy(x => x.VariantId).Select(a => new MostSaledVM
            {
                VariantId = a.Key,
                SaledQuantity = a.Sum(x => x.SaledQuantity)
            }).OrderByDescending(x => x.SaledQuantity).ToList();

            foreach (var item in orderDetails)
            {
                var product = await _context.Variants.Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == item.VariantId);

                item.ProductName = product.Product.Name;
                item.TotalSales = item.SaledQuantity * product.Price;
            };

            return orderDetails;
        }


        //Chart dount
        public async Task<CategoryChartVM> DonutChartAsync()
        {
            var Variants = await _context.OrderDetails.AsSplitQuery()
                                                   .AsNoTrackingWithIdentityResolution()
                                                   .Include(x => x.Order)
                                                   .Include(z => z.ProductVariant)
                                                   .ThenInclude(pv => pv.Product)
                                                   .ThenInclude(p => p.category)
                                                   .Where(x => x.Order.CreatedDate.Date <= DateTime.Today.Date && x.Order.CreatedDate.Date >= DateTime.Today.Date.AddDays(-30))
                                                   .GroupBy(x => x.ProductVariant.Product.category.Name ?? "nothing")
                                                   .Select(a => new 
                                                   {
                                                       Name = a.Key,
                                                       NumOfSalles = a.Sum(x => x.Quantity),
                                                   }).ToListAsync();

            var wrong1 = Variants.FirstOrDefault(x => x.Name == "nothing");
            if (wrong1 != null)
                Variants.Remove(wrong1);


            var Offers = await _context.OrderDetails.AsSplitQuery()
                                                   .AsNoTrackingWithIdentityResolution()
                                                   .Include(x => x.Order)
                                                   .Include(z => z.ProductVariant)
                                                   .ThenInclude(pv => pv.Product)
                                                   .ThenInclude(p => p.category)
                                                   .Where(x => x.Order.CreatedDate.Date <= DateTime.Today.Date && x.Order.CreatedDate.Date >= DateTime.Today.Date.AddDays(-30))
                                                   .GroupBy(x => x.OfferId ?? 0)
                                                   .Select(a => new 
                                                   {
                                                       OfferId = a.Key,
                                                       OfferQuantity = a.Sum(x => x.Quantity)
                                                   }).ToListAsync();

            var wrong2 = Offers.FirstOrDefault(x => x.OfferId == 0);
            if (wrong2 != null)
                Offers.Remove(wrong2);


            foreach (var item in Offers)
            {
                var Offer = await _context.Offers.AsSplitQuery().Include(x => x.OfferProducts)
                                                 .ThenInclude(op => op.ProductVariant).ThenInclude(pv => pv.Product)
                                                 .ThenInclude(p => p.category).FirstOrDefaultAsync(x => x.Id == item.OfferId);
                foreach (var offerproduct in Offer.OfferProducts)
                {
                    var obj = new
                    {
                        Name = offerproduct.ProductVariant.Product.category.Name,
                        NumOfSalles = item.OfferQuantity + offerproduct.Quantity,
                    };
                    Variants.Add(obj);
                }
            }

            Variants = Variants.GroupBy(x => x.Name).Select(a => new
            {
                Name = a.Key,
                NumOfSalles = a.Sum(x => x.NumOfSalles)
            }).ToList();


            List<string> Names = new List<string>();
            List<int> numbers = new List<int>();
            var model = new CategoryChartVM();

            foreach (var item in Variants)
            {
                Names.Add(item.Name);
                numbers.Add(item.NumOfSalles);
            }
            model.Names = Names;
            model.NumOfSalles = numbers;

            return model;
        }

        public async Task<List<OrderComplete_Refund_PendingChartVM>> OrderCompleteAndReturnChartVM()
        {
            List<OrderComplete_Refund_PendingChartVM> model = new();

            var today = DateTime.Today;
            var LastMonth = today.AddMonths(-11);
            for (var current = LastMonth; current <= today; current = current.AddMonths(1))
            {
                var ApprovedOrders = await _context.Orders.Where(x => x.CreatedDate.Month == current.Month && x.CreatedDate.Year == current.Year && x.paymentStatus == "Approved").CountAsync();
                var RefundOrders = await _context.Orders.Where(x => x.CreatedDate.Month == current.Month && x.CreatedDate.Year == current.Year && x.paymentStatus == "Refund").CountAsync();
                var PendingOrders = await _context.Orders.Where(x => x.CreatedDate.Month == current.Month && x.CreatedDate.Year == current.Year && x.paymentStatus == "Pending").CountAsync();
                model.Add(new OrderComplete_Refund_PendingChartVM
                {
                    //Month = Months[current.Month] +$"({current.Year})" ,
                    Month = current.ToString("MMM"),
                    ApprovedOrders = ApprovedOrders,
                    RefundOrders = RefundOrders,
                    PendingOrders = PendingOrders,
                });
            }
            return model;
        }

        public async Task<List<ProductListDetailsVM>> ProductListDetailsVMs()
        {
            var orderDetails = await _context.OrderDetails.Include(x => x.Order)
                                          .Where(x => x.Order.CreatedDate.Date <= DateTime.Today.Date && x.Order.CreatedDate.Date >= DateTime.Today.Date.AddDays(-7))
                                          .GroupBy(x => x.ProductIn_Id ?? 0).Select(a => new ProductListDetailsVM
                                          {
                                              VariantId = a.Key,
                                              QtyLeft = a.Sum(x => x.Quantity),
                                          }).ToListAsync();

            var wrong1 = orderDetails.FirstOrDefault(x => x.VariantId == 0);
            if (wrong1 != null)
                orderDetails.Remove(wrong1);

            var Offers = await _context.OrderDetails.Include(x => x.Order)
                                          .Where(x => x.Order.CreatedDate.Date <= DateTime.Today.Date && x.Order.CreatedDate.Date >= DateTime.Today.Date.AddDays(-7))
                                          .GroupBy(x => x.OfferId ?? 0).Select(a => new
                                          {
                                              OfferId = a.Key,
                                              OfferQuantity = a.Sum(x => x.Quantity)
                                          }).ToListAsync();

            var wrong2 = Offers.FirstOrDefault(x => x.OfferId == 0);
            if (wrong2 != null)
                Offers.Remove(wrong2);

            foreach (var item in Offers)
            {
                var Offer = await _context.Offers.AsSplitQuery().Include(x => x.OfferProducts).FirstOrDefaultAsync(x => x.Id == item.OfferId);
                foreach (var offerproduct in Offer.OfferProducts)
                {
                    ProductListDetailsVM obj = new ProductListDetailsVM
                    {
                        VariantId = offerproduct.ProductVariantId,
                        QtyLeft = item.OfferQuantity * offerproduct.Quantity
                    };
                    orderDetails.Add(obj);
                }
            }

            orderDetails = orderDetails.GroupBy(x => x.VariantId).Select(a => new ProductListDetailsVM
            {
                VariantId = a.Key,
                QtyLeft = a.Sum(x => x.QtyLeft)
            }).OrderByDescending(x => x.QtyLeft).ToList();

            foreach (var item in orderDetails)
            {
                var product = await _context.Variants.Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == item.VariantId);

                string statue;
                if (product.Quantity >= 15)
                    statue = "In Stuck";
                else if (product.Quantity == 0)
                    statue = "Out of Stuck";
                else
                    statue = "Low Stuck";


                item.Price = product.Price;
                item.Name = product.Product.Name + "(" + product.Color + ")";
                item.Status = statue;
            }

            return orderDetails;

        }

        #endregion





        #region Products
        //Return All Product Data 
        public async Task<dynamic> GetProductsDataAsync(ProductFilterVM filter)
        {
            var model = await _context.Products.AsSplitQuery().Include(x => x.ProductVariants).Include(c => c.category).Select(a => new
            {
                Id = a.Id,
                Name = a.Name.Take(10),
                Category = a.category.Name,
                Price = a.Price,
                Status = a.ProductVariants.Sum(pr => pr.Quantity) == 0 ? "Out of stock" : a.ProductVariants.Sum(pr => pr.Quantity) < 20 ? "Low Stock" : "In Stock",
                PaymentOption = "Cash",
                Quantity = a.ProductVariants.Sum(pr => pr.Quantity),
                Sells =_context.OrderDetails.AsSplitQuery().Include(o => o.ProductVariant).Where(x =>x.ProductVariant.Product_Id == a.Id).Sum(od =>od.Quantity)
            }).ToListAsync();

            if (filter.status != null)
            {
                if (filter.status == "All")
                {
                    model = model;
                }
                else
                {
                    model = model.Where(x => x.Status == filter.status).ToList();
                }
            }
            

            return model;
        }




        //Get Product Att based  on Product ID
        public async Task<dynamic> GetProductAttAsync(int ProductID)
        {
            //var category = await _context.categories.FirstOrDefaultAsync(x => x.Name.ToLower() == categoryName.ToLower());
            var Variant = await _context.Variants.FindAsync(ProductID);
            if(Variant != null)
            {
                var MainProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == Variant.Product_Id);
                var ProductAtt = await _context.ProductAttributes.AsSplitQuery().Include(x => x.CategoryAttribute).Where(x => x.Product_Id == MainProduct.Id).ToListAsync();

                var model = new Dictionary<string, string>();
                model.Add("Name", MainProduct.Name);
                model.Add("Description", MainProduct.Description);
                foreach (var item in ProductAtt)
                {
                    model.Add(item.CategoryAttribute.Name, item.Value);
                }
                return model;
            }
            return null;
        }













        //Get Product Status ==> { total , instuck , out }
        public async Task<dynamic> GetProductStatusAsync()
        {
            var TotalProducts = await _context.Products.CountAsync();
            var Instuck = await _context.Variants.SumAsync(x => x.Quantity);
            var Out = await _context.OrderDetails.SumAsync(x => x.Quantity);
            var model = new
            {
                total = TotalProducts,
                Instuck = Instuck,
                Out = Out
            };

            return model;
        }
        #endregion




        #region Add Product


        //Add Product
        public async Task<bool> AddProductAsync(string data, int CatId)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

                var category = await _context.categories.FindAsync(CatId);
                var CateList = await _context.categoryAttributes.Where(x => x.Category_Id == CatId).ToListAsync();


                var NewProduct = new Product
                {
                    Category_Id = CatId,
                    Description = result["Description"].ToString(),
                    IsDeleted = false,
                    Name = result["name"].ToString(),
                    Price = double.Parse(result["Price"].ToString())
                };

                await _context.Products.AddAsync(NewProduct);
                await _context.SaveChangesAsync();



                var CatAttVal = new List<ProductAttribute>();
                foreach (var catAtt in CateList)
                {
                    var val = result[catAtt.Name];
                    var obj = new ProductAttribute
                    {
                        CategoryAttribute_Id = catAtt.Id,
                        Product_Id = NewProduct.Id,
                        Value = result[catAtt.Name].ToString()
                    };
                    CatAttVal.Add(obj);
                }

                await _context.ProductAttributes.AddRangeAsync(CatAttVal);
                await _context.SaveChangesAsync();


                var Variant = new ProductVariant
                {
                    Color = result["Color"].ToString(),
                    Discount = int.Parse(result["Discount"].ToString()),
                    IsDeleted = false,
                    IsFavorite = false,
                    Price = double.Parse(result["Price"].ToString()),
                    Product_Id = NewProduct.Id,
                    Quantity = int.Parse(result["Quantity"].ToString()),
                    TotalRating = 0,
                    UserCountRating = 0,
                    ImagePath = "FB_IMG_1683728487894.jpg"
                };

                await _context.Variants.AddAsync(Variant);
                await _context.SaveChangesAsync();


            }
            catch
            {
                return false;
            }
            return true;
        }



        //Get List of Category { Id  -  Name - Attributes}
        public async Task<dynamic> GetCategoryAsync()
        {
            var model = await _context.categories.Include(x => x.CategoryAttributes).Select(a => new
            {
                Id = a.Id,
                Name = a.Name,
                Attributes = a.CategoryAttributes.Select(ca => new
                {
                    Name = ca.Name,
                })
            }).ToListAsync();
            return model;
        }




        //Save on Server and Return Image Path
        public async Task<string> SaveAndGetImagePath(IFormFile file)
        {
            var FolderPath = Path.Combine(_webHost.WebRootPath, "Images");
            var FileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var Fullpath = Path.Combine(FolderPath, FileName);
            using (var filestream = new FileStream(Fullpath, FileMode.Create))
            {
                await file.CopyToAsync(filestream);
            }
            return FileName;
        }





        //Get Product Att based  on Category
        public async Task<dynamic> GetCategoryAttAsync(string Category)
        {
            //var category = await _context.categories.FirstOrDefaultAsync(x => x.Name.ToLower() == categoryName.ToLower());
            var cat = await _context.categories.FirstOrDefaultAsync(x => x.Name.ToLower() == Category.ToLower());
            if (cat != null)
            {
                var CatAtt = await _context.categoryAttributes.Where(x => x.Category_Id == cat.Id).ToListAsync();

                var model = new Dictionary<string, string>();
                model.Add("Description", "textarea");
                model.Add("Color", "text");
                model.Add("Price", "decimal");
                model.Add("Quantity", "number");
                model.Add("Discount", "number");
                foreach (var item in CatAtt)
                {
                    model.Add(item.Name, "text");
                }
                return model;
            }
            return null;
        }

        #endregion





        #region Category

        //Get Categories Data for Category Table
        public async Task<dynamic> GetCategoriesDataAsync()
        {
            var catesells = await DonutChart();

            var categories = await _context.categories.AsSplitQuery().AsNoTracking()
                .Include(x => x.Products)
                .ThenInclude(p => p.ProductVariants)
                .ThenInclude(pv => pv.OrderDetails)
                .ToListAsync();

            var model = categories.Select(a => new
            {
                Id = a.Id,
                Name = a.Name,
                p_num = a.Products.Count(),
                quantity = a.Products.Sum(x => x.ProductVariants.Sum(pv => pv.Quantity)),
                Sells = catesells.Any(x => x.Key == a.Name)
                    ? catesells.First(x => x.Key == a.Name).Value
                    : 0
            }).ToList();

            return model;
        }
        private async Task<List<KeyValuePair<string, int>>> DonutChart()
        {
            var Variants = await _context.OrderDetails.AsSplitQuery()
                                                   .AsNoTrackingWithIdentityResolution()
                                                   .Include(x => x.Order)
                                                   .Include(z => z.ProductVariant)
                                                   .ThenInclude(pv => pv.Product)
                                                   .ThenInclude(p => p.category)
                                                   .Where(x => x.Order.CreatedDate.Date <= DateTime.Today.Date && x.Order.CreatedDate.Date >= DateTime.Today.Date.AddDays(-30))
                                                   .GroupBy(x => x.ProductVariant.Product.category.Name ?? "nothing")
                                                   .Select(a => new
                                                   {
                                                       Name = a.Key,
                                                       NumOfSalles = a.Sum(x => x.Quantity),
                                                   }).ToListAsync();

            var wrong1 = Variants.FirstOrDefault(x => x.Name == "nothing");
            if (wrong1 != null)
                Variants.Remove(wrong1);


            var Offers = await _context.OrderDetails.AsSplitQuery()
                                                   .AsNoTrackingWithIdentityResolution()
                                                   .Include(x => x.Order)
                                                   .Include(z => z.ProductVariant)
                                                   .ThenInclude(pv => pv.Product)
                                                   .ThenInclude(p => p.category)
                                                   .Where(x => x.Order.CreatedDate.Date <= DateTime.Today.Date && x.Order.CreatedDate.Date >= DateTime.Today.Date.AddDays(-30))
                                                   .GroupBy(x => x.OfferId ?? 0)
                                                   .Select(a => new
                                                   {
                                                       OfferId = a.Key,
                                                       OfferQuantity = a.Sum(x => x.Quantity)
                                                   }).ToListAsync();

            var wrong2 = Offers.FirstOrDefault(x => x.OfferId == 0);
            if (wrong2 != null)
                Offers.Remove(wrong2);


            foreach (var item in Offers)
            {
                var Offer = await _context.Offers.AsSplitQuery().Include(x => x.OfferProducts)
                                                 .ThenInclude(op => op.ProductVariant).ThenInclude(pv => pv.Product)
                                                 .ThenInclude(p => p.category).FirstOrDefaultAsync(x => x.Id == item.OfferId);
                foreach (var offerproduct in Offer.OfferProducts)
                {
                    var obj = new
                    {
                        Name = offerproduct.ProductVariant.Product.category.Name,
                        NumOfSalles = item.OfferQuantity + offerproduct.Quantity,
                    };
                    Variants.Add(obj);
                }
            }

            var DataFormat = Variants
                        .GroupBy(x => x.Name)
                        .Select(a => new KeyValuePair<string, int>(
                            a.Key,
                            a.Sum(x => x.NumOfSalles)
                        ))
                        .ToList();
            return DataFormat;
        }





        //Delete Attribute based on ID
        public async Task<string> DeleteAttributeAsync(int attributeId)
        {
            string error = null;
            var item = await _context.categoryAttributes.FindAsync(attributeId);
            if (item != null)
            {
                try
                {
                    _context.categoryAttributes.Remove(item);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    error = "This Attribute Already Exist in Other Products";
                    return error;
                }
            }
            return error;
        }





        //Add new Attribute to Certain Category
        public async Task<bool> AddNewAttributeAsync(int CategoryId, string AddedField)
        {
            var old = await _context.categoryAttributes.FirstOrDefaultAsync(x => x.Category_Id == CategoryId && x.Name.ToLower() == AddedField.ToLower());
            if (old != null) return false;
            var item = new CategoryAttribute
            {
                Name = AddedField,
                Category_Id = CategoryId
            };
            await _context.categoryAttributes.AddAsync(item);
            await _context.SaveChangesAsync();
            return true;
        }





        //Return All Attributes For Category Based on Its Id
        public async Task<dynamic> GetCategoryAttributeAsync(int CategoryId)
        {
            var model = await _context.categoryAttributes.Where(x => x.Category_Id == CategoryId)
                                      .Select(a => new
                                      {
                                          Id = a.Id,
                                          Name = a.Name,
                                      }).ToListAsync();
            return model;
        }




        //Add Category
        public async Task<int> AddCategoryAsync(string Categoryname)
        {
            var item = new Category
            {
                Name = Categoryname,
            };

            await _context.categories.AddAsync(item);
            await _context.SaveChangesAsync();
            return item.Id;
        }






        //Delete Category and its attribute 
        public async Task<string> DeleteCategoryAsync(int CategoryId)
        {
            string error = null;
            var category = await _context.categories.FindAsync(CategoryId);
            if (category != null)
            {
                try
                {
                    category.IsDeleted = true;
                    _context.categories.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    error = "The Category or its attributes used by Products";
                }
            }

            return error;
        }




        //Add Category & Its Attributes
        public async Task<bool> AddNewCategoryAsync(string obj)
        {
            var result = JsonConvert.DeserializeObject<AddCategoryVM>(obj);

            if (result.name == null)
                return false;

            var old = await _context.categories.FirstOrDefaultAsync(x => x.Name.ToLower() == result.name.ToLower());
            if (old != null) return false;

            var category = new Category
            {
                Name = result.name,
            };
            await _context.categories.AddAsync(category);
            await _context.SaveChangesAsync();


            var categoryAttributes = new List<CategoryAttribute>();
            if(result.attributes != null)
            {
                foreach (var attribute in result.attributes)
                {
                    var item = new CategoryAttribute
                    {
                        Category_Id = category.Id,
                        Name = attribute
                    };
                    categoryAttributes.Add(item);
                }
                await _context.categoryAttributes.AddRangeAsync(categoryAttributes);
                await _context.SaveChangesAsync();
            }
            return true;
            
        }
        #endregion





        #region Order

        //Return The Order Table Data
        public async Task<dynamic> GetOrderDataAsync()
        {
            var model = await _context.Orders.Include(x => x.status).Include(z => z.OrderDetails).Select(a => new
            {
                Id = a.Id,
                createdDay = a.CreatedDate.ToString("yyyy-mm-dd"),
                paymentMethod = a.paymentMethod,
                paymentStatus = a.paymentStatus,
                orderStatus = a.status.Name,
                total = a.OrderDetails.Sum(x => x.Quantity * x.UnitPrice)
            }).ToListAsync();

            return model;
        }





        //Get Order Details 
        public async Task<dynamic> GetOrderDetailsAsync(int OrderId)
        {
            var Order = await _context.Orders.AsSplitQuery().AsNoTracking().Include(x => x.User).Include(a => a.status).FirstOrDefaultAsync(x => x.Id ==  OrderId);
            if (Order == null) return null;

            var OrderDetailsItem = await _context.OrderDetails.AsSplitQuery().AsNoTracking().Include(x => x.ProductVariant).ThenInclude(pv => pv.Product).Where(x => x.Order_Id == OrderId)
                .Where(x => x.ProductIn_Id != null).Select(a => new
            {
                VariantId = a.ProductIn_Id,
                Name = a.ProductVariant.Product.Name,
                Image = a.ProductVariant.ImagePath,
                Quantity = a.Quantity,
                Price = a.UnitPrice
            }).ToListAsync();


            var OrderDetailsOffer = await _context.OrderDetails.AsSplitQuery().AsNoTracking().Include(x => x.Offer).ThenInclude(o => o.OfferProducts).ThenInclude(op => op.ProductVariant).ThenInclude(pv => pv.Product).Where(x => x.Order_Id == OrderId)
                .Where(x => x.OfferId != null).Select(a => new
                {
                    VariantId = a.OfferId,
                    Name = a.Offer.Name,
                    Quantity = a.Quantity,
                    Price = a.UnitPrice,
                    Images = a.Offer.OfferProducts.Select(a => a.ProductVariant.ImagePath),
                    names = a.Offer.OfferProducts.Select(a => a.ProductVariant.Product.Name),
                }).ToListAsync();


            var model = new
            {
                UserId = Order.User_Id,
                UserName = Order.User.FirstName +" "+ Order.User.LastName,
                Email = Order.User.Email,
                Phone = Order.User.PhoneNumber,
                UserImage = Order.User.ImagePath,
                CreatedAt = Order.CreatedDate.Date.ToString("yyyy-dd-MM"),
                OrderStatus = Order.status.Name,
                OrderDetailsItem = OrderDetailsItem,
                OrderDetailsOffer = OrderDetailsOffer,
                total = Math.Round(OrderDetailsItem.Sum(x => x.Quantity * x.Price) + OrderDetailsOffer.Sum(x => x.Quantity * x.Price), 2)
            };

            return model;
            
        }
        #endregion






        #region Users

        //Return Users Data ==> Json Format
        public async Task<dynamic> GetUsersDataAsync(UpdateRoleVM result)
        {
            var model = await (from userrole in _context.UserRoles
                               join role in _context.Roles on userrole.RoleId equals role.Id
                               join user in _context.Users on userrole.UserId equals user.Id
                               select new
                               {
                                   Id = user.Id,
                                   Name = user.FirstName +" "+ user.LastName,
                                   Phone =  user.PhoneNumber,
                                   createdDate = user.CreatedAt.Value.Date.ToString("yyyy-mm-dd"),
                                   Role = role.Name
                               }).ToListAsync();

            if(result.Role != null)
            {
                if(result.Role == "All")
                {
                    model = model;
                }
                else
                {
                    model = model.Where(x => x.Role.ToLower() == result.Role.ToLower()).ToList();
                }
            }


            return model;
        }




        //Update The Role Assigned to User 
        public async Task<bool> UpdateUserRoleAsync(string obj)
        {
            var model = JsonConvert.DeserializeObject<UpdateRoleVM>(obj);
            if (model.id == null || model.Role == null)
                return false;

            var user = await _context.Users.FindAsync(model.id);
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Name.ToLower() == model.Role.ToLower());


            if (user == null || role == null)
                return false;


            var userrole = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId ==  user.Id);

            _context.UserRoles.Remove(userrole);
            await _context.SaveChangesAsync();

            var item = new IdentityUserRole<string>
            {
                RoleId = role.Id,
                UserId = user.Id
            };

            await _context.UserRoles.AddAsync(item);
            await _context.SaveChangesAsync();
            return true;
        }



        //Delete User Account 
        public async Task<bool> DeleteUserAsync(string UserId)
        {
            var user = await _context.Users.FindAsync(UserId);
            if(user == null) return false;

            user.IsDeleted = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
