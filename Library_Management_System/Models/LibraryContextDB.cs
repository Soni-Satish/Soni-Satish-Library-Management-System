using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Models
{
    public class LibraryContextDB : DbContext
    {
        public LibraryContextDB(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookTransaction> BookTransactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
