using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.Models
{
    public class ProductVariantImage
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ProductVariant")]
        [Display(Name = "ProductVariant")]
        public int ProductVariant_Id { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }
        public string ImagePath { get; set; }
    }
}
