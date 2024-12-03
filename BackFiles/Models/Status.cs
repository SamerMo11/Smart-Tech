using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
