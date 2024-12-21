using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Models
{
    public class User
    {
        [Key]  
        public Guid UserID { get; set; }  

        [Required]  
        [StringLength(100)]  
        public string Name { get; set; }

        [Required] 
        [StringLength(100)]  
        [EmailAddress]  
        public string Email { get; set; }

        [Phone]  
        [StringLength(20)] 
        public string Phone { get; set; }
    }

}
