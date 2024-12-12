using E_CommerceWeb.ViewModels.Dashboard.MainPage;
using E_CommerceWeb.ViewModels.Dashboard.Product;
using E_CommerceWeb.ViewModels.Dashboard.User;

namespace E_CommerceWeb.Repository.Interface
{
    public interface IDashboardRepo
    {


        #region AddProduct
        //Get List of Category { Id  -  Name}
        Task<dynamic> GetCategoryAsync();



        //Save on Server and Return Image Path
        Task<string> SaveAndGetImagePath(IFormFile file);




        //Get Product Att based  on Category
        Task<dynamic> GetCategoryAttAsync(string Category);




        //Add Product
        Task<bool> AddProductAsync(string data, int CatId);
        #endregion



        #region Product

        //Return All Product Data 
        Task<dynamic> GetProductsDataAsync(ProductFilterVM filter);





        //Get Product Att based  on Category
        Task<dynamic> GetProductAttAsync(int ProductID);






        //Get Product Status ==> { total , instuck , out }
        Task<dynamic> GetProductStatusAsync();
        #endregion



        #region MainDashboard
        //Combination of Data In The Main Page in Dashboard
        Task<MainPageVM> MainPageDataAsync();
        #endregion



        #region Category
        //Get Categories Data for Category Table
        Task<dynamic> GetCategoriesDataAsync();





        //Delete Attribute based on ID
        Task<string> DeleteAttributeAsync(int attributeId);



        //Add new Attribute to Certain Category
        Task<bool> AddNewAttributeAsync(int CategoryId, string AddedField);






        //Return All Attributes For Category Based on Its Id
        Task<dynamic> GetCategoryAttributeAsync(int CategoryId);





        //Add Category
        Task<int> AddCategoryAsync(string Categoryname);






        //Delete Category and its attribute 
        Task<string> DeleteCategoryAsync(int CategoryId);





        //Add Category & Its Attributes
        Task<bool> AddNewCategoryAsync(string obj);
        #endregion



        #region Order
        //Return The Order Table Data
        Task<dynamic> GetOrderDataAsync();



        //Get Order Details 
        Task<dynamic> GetOrderDetailsAsync(int OrderId);


        #endregion




        #region User

        //Return Users Data ==> Json Format
        Task<dynamic> GetUsersDataAsync(UpdateRoleVM result);




        //Update The Role Assigned to User 
        Task<bool> UpdateUserRoleAsync(string obj);




        //Delete User Account 
        Task<bool> DeleteUserAsync(string UserId);
        #endregion
    }
}
