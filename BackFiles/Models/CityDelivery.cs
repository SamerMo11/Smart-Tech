using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.Models
{
    public class CityDelivery
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City City { get; set; }


        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int Tasks { get; set; }
    }
}
