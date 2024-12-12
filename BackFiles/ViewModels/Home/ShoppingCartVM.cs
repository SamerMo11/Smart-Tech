using E_CommerceWeb.Models;

namespace E_CommerceWeb.ViewModels.Home
{
    public class ShoppingCartVM
    {
        public int Cart_ID { get; set; }
        public List<CartDetailsItemVM> cartDetailsItemVMs { get; set; }
        public List<CartDetailsOfferVM> cartDetailsOfferVMs { get; set; }
    }




    public class CartDetailsItemVM
    {
        public int CartDetail_Id { get; set; }
        public int ProductVAriant_Id { get; set; }
        public int QuantityInCart { get; set; }
        public string Color { get; set; }
        public int Discount { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string ImagePath { get; set; }

        public Product Product { get; set; }
    }


    public class CartDetailsOfferVM
    {
        public int CartDetail_Id { get; set; }
        public int OfferId { get; set; }
        public int QuantityInCart { get; set; }
        public List<string> Names { get; set; }
        public string Discription { get; set; }
        public string MainName { get; set; }
        public double Price { get; set; }
        public List<string> ImagePaths { get; set; }
    }

}
