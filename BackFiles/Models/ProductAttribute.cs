using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.Models
{
    public class ProductAttribute
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Product")]
        public int Product_Id { get; set; }
        [ForeignKey("CategoryAttribute")]
        public int CategoryAttribute_Id { get; set; }
        public virtual Product Product { get; set; }
        public virtual CategoryAttribute CategoryAttribute { get; set; }
        public string Value { get; set; }
    }
}
