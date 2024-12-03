using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.Models
{
    public class Offer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public List<OfferProduct> OfferProducts { get; set; }
    }
}
