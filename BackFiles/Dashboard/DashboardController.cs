using E_CommerceWeb.Data;
using E_CommerceWeb.Models;
using E_CommerceWeb.Repository.Interface;
using E_CommerceWeb.ViewModels.Dashboard.Category;
using E_CommerceWeb.ViewModels.Dashboard.Product;
using E_CommerceWeb.ViewModels.Dashboard.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace E_CommerceWeb.Controllers
{
    [Authorize(Roles = "admin,manager")]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepo _dashboardRepo;
        private readonly AppDbContext _context;

        public DashboardController(IDashboardRepo dashboardRepo,
                                   AppDbContext context)
        {
            _dashboardRepo = dashboardRepo;
            _context = context;
        }





        public async Task<IActionResult> AddAdd(IFormFile file,int id)
        {
            var variant = await _context.Variants.FindAsync(id);
            variant.ImagePath = await _dashboardRepo.SaveAndGetImagePath(file);
            await _context.SaveChangesAsync();
            return RedirectToAction("Home","Home");
        }


        public async Task<IActionResult> AddAddd(IFormFile file, int id)
        {
            var xx = new ProductVariantImage
            {
                ImagePath = await _dashboardRepo.SaveAndGetImagePath(file),
                ProductVariant_Id = id
            };
            await _context.VariantImages.AddAsync(xx);
            await _context.SaveChangesAsync();
            return RedirectToAction("Home", "Home");
        }



        /////////////////////////////////////////////////////////////////    Pages       ///////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //Return the main page
        public IActionResult MainDashboard()
        {
            return View();
        }





        //Return the Order page
        public IActionResult Orders()
        {
            return View();
        }



        //Return the Product page
        public IActionResult Product()
        {
            return View();
        }




        //Return the Product page
        public IActionResult AddProduct()
        {
            return View();
        }


        //Return the Category page
        public IActionResult Category()
        {
            return View();
        }





        //Return the Users page
        public IActionResult User()
        {
            return View();
        }

        /////////////////////////////////////////////////////////////////    Logic       ///////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region MainDashboard
        // Data In the Main page in dashboard
        public async Task<IActionResult> GetMainDashboardData()
        {
            var model = await _dashboardRepo.MainPageDataAsync();
            return Json(model);
        }
        #endregion



        #region Product

        //Return All Product Data 
        public async Task<IActionResult> GetProductsData(string  filter)
        {
            var result = JsonConvert.DeserializeObject<ProductFilterVM>(filter);
            var model = await _dashboardRepo.GetProductsDataAsync(result);
            return Json(model);
        }



        //Return The Attributes of Category ==> Name
        public async Task<IActionResult> GetAttributes(int productId)
        {
            if (productId == null) return Json(null);
            var model = await _dashboardRepo.GetProductAttAsync(productId);
            return Json(model);
        }





        //Get Product Status ==> { total , instuck , out }
        public async Task<IActionResult> GetProductStatus()
        {
            var model = await _dashboardRepo.GetProductStatusAsync();
            return Json(model); 
        }
        #endregion


        #region AddProduct
        //Return the Whole Category in Json Format
        public async Task<IActionResult> GetCategory()
        {
            var model = await _dashboardRepo.GetCategoryAsync();
            return Json(model);
        }





        //Get Att Based on Category name
        public async Task<IActionResult> GetCategoryAtt(string name)
        {
            if(name == null) return Json(null);
            var model = await _dashboardRepo.GetCategoryAttAsync(name);
            return Json(model);
        }






        //Add Product 
        public async Task<IActionResult> AddNewProduct(string data,int CatId)
        {


            var result = await _dashboardRepo.AddProductAsync(data, CatId);
            if(result == false) return Json(false);

            return Json(true);
        }
        #endregion


        #region Category
        //Get Categories Data for Category Table
        public async Task<IActionResult> GetCategoriesData()
        {
            var model = await _dashboardRepo.GetCategoriesDataAsync();
            return Json(model);
        }

            

        //Delete Attribute 
        public async Task<IActionResult> DeleteCategoryAttribute(int attributeId)
        {
            if (attributeId == 0)
                return NotFound("Not Valid Value");

            var result = await _dashboardRepo.DeleteAttributeAsync(attributeId);
            if (result == null)
                return Json(true);
            return Json(false, result);
        }




        //Add new Attribute to Certain Category
        public async Task<IActionResult> AddNewAttribute(int CategoryId, string AddedField)
        {
            if (CategoryId != 0 && !string.IsNullOrEmpty(AddedField))
            {
                var result =  await _dashboardRepo.AddNewAttributeAsync(CategoryId, AddedField);
                if (result == false) return Json(false); 
                return Json(true);
            }
            return Json(false);
        }




        //Return All Attributes For Category Based on Its Id
        public async Task<IActionResult> GetCategoryAttribute(int categoryId)
        {
            if (categoryId == 0)
                return NotFound();
            var model = await _dashboardRepo.GetCategoryAttributeAsync(categoryId);
            return Json(model);
        }




        //Add Category
        [HttpPost]
        public async Task<IActionResult> AddCategory(string newCatName)
        {
            if (!string.IsNullOrEmpty(newCatName))
            {
                var id = await _dashboardRepo.AddCategoryAsync(newCatName);
                return Json(true);
            }
            return Json(false);
        }






        //Delete Category
        public async Task<IActionResult> DeleteCategory(int CategoryId)
        {
            if (CategoryId != 0)
            {
                var result = await _dashboardRepo.DeleteCategoryAsync(CategoryId);
                if (result == null)
                {
                    return Json(true);
                }
            }
            return Json(false);
        }





        //Add Category & Its Attributes
        public async Task<IActionResult> AddNewCategory(string obj)
        {
            if(obj  == null)
                return Json(true);

            var result = await _dashboardRepo.AddNewCategoryAsync(obj);
            if(result == false)
                return Json(true);

            return Json(true);
        }

        #endregion


        #region Order

        //Return The Order Table Data
        public async Task<IActionResult> GetOrderData()
        {
            var model = await _dashboardRepo.GetOrderDataAsync();
            return Json(model);
        }



        //Get Order Details 
        public async Task<IActionResult> GetOrderDetails(string OrderId)
        {
            if (OrderId == "0") return NotFound();
            var num = JsonConvert.DeserializeObject<int>(OrderId);
            var model = await _dashboardRepo.GetOrderDetailsAsync(num);
            return Json(model);
        }
        #endregion


        #region Users
        //get User Data ==> User Page
        public async Task<IActionResult> GetUsersData(string role)
        {
            var result = JsonConvert.DeserializeObject<UpdateRoleVM>(role);
            var model = await _dashboardRepo.GetUsersDataAsync(result);
            return Json(model);
        }




        //Update The Role Assigned to User 
        public async Task<IActionResult> UpdateUserRole(string obj)
        {
            if (obj != null) 
            {
                var result = await _dashboardRepo.UpdateUserRoleAsync(obj);
                if (result == true)
                    return Json(result);
            }
            return Json(false);
        }




        //Delete User Account 
        public async Task<IActionResult> DeleteUser(string id)
        {
            if(id == null) return Json(false);

            var result = await _dashboardRepo.DeleteUserAsync(id);
            if (result == false) return Json(false);
            return Json(true);
        }
        #endregion

    }
}
