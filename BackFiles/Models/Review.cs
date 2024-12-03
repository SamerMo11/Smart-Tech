using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
        [ForeignKey("ProductVariant")]
        public int productVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }


        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int RateValue { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
