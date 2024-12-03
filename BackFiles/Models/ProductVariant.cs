using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.Models
{
    public class ProductVariant
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Product")]
        [Display(Name = "Product")]
        public int Product_Id { get; set; }
        public virtual Product Product { get; set; }
        public string Color { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public string ImagePath { get; set; }
        public int? UserCountRating { get; set; } = 1;
        public int? TotalRating { get; set; } = 1;
        public virtual ICollection<ProductVariantImage> VariantImages { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails{ get; set; }
        [NotMapped]
        public bool IsFavorite { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
