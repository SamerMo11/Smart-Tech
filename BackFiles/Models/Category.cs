using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace E_CommerceWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
        public virtual ICollection<CategoryAttribute>? CategoryAttributes { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
