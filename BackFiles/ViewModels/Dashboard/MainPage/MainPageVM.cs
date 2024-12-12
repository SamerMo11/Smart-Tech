namespace E_CommerceWeb.ViewModels.Dashboard.MainPage
{
    public class MainPageVM
    {
        public dynamic TotalSales { get; set; }
        public dynamic TotalRefundOrders { get; set; }
        public dynamic TotalApprovedOrders { get; set; }
        public List<MostSaledVM> MostSelling { get; set; }
        public CategoryChartVM OrdersByCategory { get; set; }
        public List<OrderComplete_Refund_PendingChartVM> OrderAnalytics { get; set; }
        public List<ProductListDetailsVM> productList { get; set; }
    }
}
