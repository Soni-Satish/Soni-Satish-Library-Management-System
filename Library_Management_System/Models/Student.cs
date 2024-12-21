using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Management_System.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Column("BookName", TypeName = "varchar(100)")]
        [Required]
        public string BookName { get; set; }

        [Column("BookAuthorName", TypeName = "varchar(20)")]
        [Required]
        public string Author { get; set; }

        [Column("BookEdition", TypeName = "varchar(20)")]
        public string? Edition { get; set; }

        [Column("BookPublisher", TypeName = "varchar(20)")]
        public string? Publisher { get; set; }

        [Column("BookPrice")]
        public float? Price { get; set; }
    }
}
