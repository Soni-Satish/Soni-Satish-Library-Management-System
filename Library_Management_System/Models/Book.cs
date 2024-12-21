using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Management_System.Models
{
    public class Book
    {
        [Key]  
        public Guid BookID { get; set; }  

        [Required]  
        [StringLength(100)]  
        public string BookName { get; set; }

        [Required]  
        [StringLength(100)]  
        public string Author { get; set; }

        [StringLength(300)]
        public string? Edition { get; set; }
        [StringLength(300)]
        public string? Publisher { get; set; }
        public float? Price { get; set; }

        public DateTime PublishedDate { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        [NotMapped]
        public int AvailableCopies { get; set; }
    }
}
