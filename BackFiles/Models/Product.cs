using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(150)]
        public string Description { get; set; }
        public double Price { get; set; }
        [ForeignKey("category")]
        [Display(Name = "Category")]
        public int Category_Id { get; set; }
        public virtual Category? category { get; set; }
        public virtual ICollection<ProductVariant>? ProductVariants { get; set; }
        public virtual ICollection<ProductAttribute>? ProductAttributes { get; set; }
        public bool? IsDeleted { get; set; } = false;

        //  [NotMapped]
        //  public AddProductVM? details { get; set; }
    }
}
