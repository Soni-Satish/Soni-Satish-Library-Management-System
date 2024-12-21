using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Models
{
    public class BookTransaction
    {
        [Key]  
        public Guid TransactionID { get; set; }  

        [Required] 
        public Guid BookID { get; set; } 

        [Required] 
        public Guid UserID { get; set; } 

        [Required]  
        public TransactionType TransactionType { get; set; }  // 'Issue' or 'Return'

        public DateTime TransactionDate { get; set; }

        // Foreign Key relationships
        [ForeignKey("BookID")]
        public Book Book { get; set; }

    }
    public enum TransactionType
    {
        Issue = 1,
        Return = 2
    }
}
