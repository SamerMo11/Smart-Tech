using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.Models
{
    public class CategoryAttribute
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("category")]
        [Display(Name = "Category")]
        public int Category_Id { get; set; }
        public virtual Category category { get; set; }
    }
}
