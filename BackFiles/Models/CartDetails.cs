using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace E_CommerceWeb.Models
{
    public class CartDetails 
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ShoppingCart")]
        [Display(Name = "Shopping Cart")]
        public int ShoppingCart_Id { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }
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
