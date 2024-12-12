using E_CommerceWeb.Models;
using E_CommerceWeb.ViewModels.Home;
using Stripe.Checkout;

namespace E_CommerceWeb.Repository.Interface
{
    public interface IHomeRepo
    {
        //Get User Id
        string GetUserId();


        #region Contact
        //Save Contact Fields in DB
        Task<bool> AddContactAsync(Contact model);
        #endregion



        #region Home & Cart
        //Method for Count of items in the cart ===> according to Specific User
        Task<int> GetCartCountAsync();



        //Method for Adding Item to Cart
        Task<int> AddItemAsync(int productvariantid, int quantity);




        //Method for Adding Offer to Cart
        Task<int> AddOfferAsync(int offerid, int quantity = 1);



        //Method for Deleting Item to Cart
        Task<int> DeleteItemAsync(int productvariantid);



        //Method for Deleting Offer form Cart
        Task<int> DeleteOfferAsync(int Offerid);



        //Delete All Item From Cart Details
        Task<bool> DeleteAllItemAsync(int ProductId);



        //Delete All Offer From Cart Details
        Task<bool> DeleteAllOfferAsync(int OfferId);



        //Get Data In The Home Page
        Task<dynamic> GetHomeDataAsync();



        //Last Searches For User
        Task<dynamic> GetLastSearchesAsync();



        //Return Suggested Products
        Task<dynamic> GetSearchProductAsync(string text);




        //Delete Search Result from Search Model At {Home}
        Task<bool> DeleteSearchResultAsync(int SearchId);



        //Data in Shopping Cart
        Task<dynamic> GetCartDataAsync();






        //Method For Add Or Remove Product From His Favorite List
        Task AddOrRemoveFavoriteProductAsync(int productId, bool isChecked);





        //update Quantity
        Task<int> UpdateItemQuantityAsync(int productvariantid, int offerId, int quantity);
        #endregion


        #region product View
        //Get Data in product View ==> {Details - define MainAttribute - Related Product}
        Task<dynamic> GetProductAsync(int mainProductId, int? variantId);





        //Check Validation To allow making CheckOut
        Task<dynamic> CheckReviewValidationAsync(int productId);




        //Make Review 
        Task<dynamic> MakeReviewAsync(string Message, int RateValue, int VariantId);
        #endregion



        #region Shop
        //Return The Shop data
        Task<dynamic> GetShopDataAsync(ShopFilter filter);
        #endregion


        #region Check Out
        //Get All Cart Details Data
        Task<ShoppingCartVM> GetCartDetailsAsync();



        // View And Keep Data in Check Out Page And Make The Whole Model 
        Task<CheckOutVM> GetCheckOutDataAsync();



        // Get Cites By State_ID 
        Task<IEnumerable<City>> GetCityByStateIdAsync(int stateid);
        // return States
        Task<List<state>> GetStatesAsync();



        //Check that Code Is Valid (Check Out Page)
        Task<decimal> CheckCodeValidationAndGetDiscountAsync(string code);
        // Check Existing Code Only
        Task<bool> CheckCodeValidationAsync(string code);



        //make Order and It's Details and clear Cart Details {Cash}
        Task MakeOrderCashAsync(string PaymentMethod, string phone, string address, int cityId, bool HasDiscount);
        //make Order and It's Details and clear Cart Details {OnLine}
        Task MakeOrderOnLineAsync(string PaymentMethod, string phone, string address, int cityId, string StripeSessionId, string StripePaymentIntentId, bool HasDiscount);



        //make checkout and move to stripe web
        Task<SessionCreateOptions> CheckOutAsync(string code);



        // Get DiscountCode By Name and update IsUsed To ===> TRUE
        Task GetDiscountCodeByCodeAsync(string code);
        #endregion
    }
}
