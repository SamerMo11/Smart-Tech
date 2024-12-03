using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceWeb.Models
{
    public class OfferProduct
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Offer")]
        public int OfferId { get; set; }
        public Offer Offer { get; set; }
        [ForeignKey("ProductVariant")]
        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }
        public int Quantity { get; set; }
    }
}
