using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceWeb.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("state")]
        [Display(Name = "State")]
        public int State_Id { get; set; }
        public virtual state state { get; set; }
    }
}
