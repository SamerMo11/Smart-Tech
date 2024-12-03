using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        [Required(ErrorMessage = "Name is required...")]
        [MaxLength(100)]
        public string Name { get; set; }
        public string? Company { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone is required...")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required...")]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string? Subject { get; set; }
        [Required(ErrorMessage = "Message is required...")]
        [MaxLength(300)]
        public string Message { get; set; }

    }
}
