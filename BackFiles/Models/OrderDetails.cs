using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.Models
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Order")]
        [Display(Name = "Order")]
        public int Order_Id { get; set; }
        public virtual Order Order { get; set; }
        [ForeignKey("ProductVariant")]
        [Display(Name = "ProductVariant")]
        public int? ProductIn_Id { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        [ForeignKey("Offer")]
        public int? OfferId { get; set; }
        public Offer Offer { get; set; }
    }
}
