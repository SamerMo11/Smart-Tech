using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.Models
{
    public class state
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<City> City { get; set; }
    }
}
