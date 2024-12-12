using E_CommerceWeb.Models;

namespace E_CommerceWeb.ViewModels.Account.DashData
{
    public class AccountVM
    {
        
        public int CartNumber { get; set; }
        public int WishNumber { get; set; }


        public List<AccountOrderVM> accountOrders{ get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }


    public class AccountOrderVM
    {
        public int OrderId{ get; set; }
        public string paymentStatus { get; set; }
        public int ProductCounts { get; set; }
        public DateTime CreatedDate { get; set; }
        public string StatusName { get; set; }
        public int StatusId { get; set; }
        public List<AccountOrderDetailsVM> accountOrderDetails{ get; set; }
    }

    public class AccountOrderDetailsVM()
    {
        public string ProductName{ get; set; }
        public int Quantity{ get; set; }
        public double Price{ get; set; }
        public string Category { get; set; }
        public string ImagePath { get; set; }

    }

}
