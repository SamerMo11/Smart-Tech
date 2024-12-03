using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string User_Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        [ForeignKey("status")]
        [Display(Name = "Status")]
        public int Status_id { get; set; }
        public Status status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public string Phone { get; set; }
        [ForeignKey("City")]
        [Display(Name = "City")]
        public int City_Id { get; set; }
        public virtual City City { get; set; }
        public string Address { get; set; }
        [ForeignKey("Delivery")]
        public string? Carrier { get; set; }
        public virtual ApplicationUser Delivery { get; set; }

        [Display(Name = "Tracking Number")]
        [MaxLength(8)]
        public string? Trackingnumber { get; set; }
        public string? paymentStatus { get; set; }
        public string? paymentMethod { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        public string? StripeSessionId { get; set; }
        public string? StripePaymentIntentId { get; set; }
        public bool? HasDiscount { get; set; } = false;
        public bool? DeliveryAccepted { get; set; }
    }
}
