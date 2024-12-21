using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;

namespace Library_Management_System.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly LibraryContextDB _libraryContextDB;

        public HomeController(LibraryContextDB libraryContextDB)
        {
            _libraryContextDB = libraryContextDB;
        }

        public async Task<IActionResult> Index()
        {
            var bookData = await _libraryContextDB.Books.ToListAsync();
            bookData = await FindAvailableBooks(bookData);
            return View(bookData);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string searchTerm)
        {
            if(!string.IsNullOrEmpty(searchTerm))
            {
                var result = await _libraryContextDB.Books.Where(b => b.BookName.Contains(searchTerm) || b.Author.Contains(searchTerm)).ToListAsync();
                result = await FindAvailableBooks(result);
                return View(result);
            }
            else
            {
                var result = await _libraryContextDB.Books.ToListAsync();
                result = await FindAvailableBooks(result);
                return View(result);
            }
        }
        private async Task<List<Book>> FindAvailableBooks(List<Book> bookData)
        {
            var booktrans = await _libraryContextDB.BookTransactions.ToListAsync();
            foreach (var item in bookData)
            {
                var trans = booktrans.FindAll(x => x.BookID == item.BookID);
                if (trans.Count > 0)
                {
                    int totalIssued = trans.FindAll(t => t.TransactionType == TransactionType.Issue).Count();
                    int totalReturned = trans.FindAll(t => t.TransactionType == TransactionType.Return).Count();
                    if (totalIssued > 0)
                    {
                        item.AvailableCopies = totalIssued - totalReturned;
                        item.AvailableCopies = item.AvailableCopies >= 0 ? item.AvailableCopies : 0;
                    }
                }

            }
            return bookData;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book std)
        {
            if(ModelState.IsValid)
            {
                await _libraryContextDB.Books.AddAsync(std);
                await _libraryContextDB.SaveChangesAsync();
                TempData["insert_success"] = "Inserted....";
                return RedirectToAction("Index","Home");
            }
            return View(std);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if(id == null || _libraryContextDB.Books == null)
            {
                return NotFound();  
            }
            var bookData = await _libraryContextDB.Books.FirstOrDefaultAsync(x=>x.BookID == id);

            if(bookData == null)
            {
                return NotFound();
            }

            return View(bookData);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            var bookData = await _libraryContextDB.Books.FindAsync(id);
            if (bookData == null) 
            {
                return NotFound();
            }
            return View(bookData);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid? id, Book std)
        {
            if(id != std.BookID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _libraryContextDB.Books.Update(std); 
                await _libraryContextDB.SaveChangesAsync();
                TempData["update_success"] = "Updated....";
                return RedirectToAction("Index", "Home");
            }
            return View(std);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _libraryContextDB.Books == null)
            {
                return NotFound();
            }
            var bookData = await _libraryContextDB.Books.FirstOrDefaultAsync(x => x.BookID == id);
            if (bookData == null)
            {
                return NotFound();
            }
            return View(bookData);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            var bookData = await _libraryContextDB.Books.FindAsync(id);
            if(bookData != null)
            {
                _libraryContextDB.Books.Remove(bookData); 
            }
            await _libraryContextDB.SaveChangesAsync();
            TempData["delete_success"] = "Deleted....";
            return RedirectToAction("Index", "Home");   
        }
        public async Task<IActionResult> Issue(Guid id)
        {
            if (id == Guid.Empty || _libraryContextDB.BookTransactions == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var book = _libraryContextDB.Books.FirstOrDefault(b => b.BookID == id);
            if (book != null && book.Quantity > 0)
            {
                BookTransaction bookTransaction = new BookTransaction
                {
                    BookID = id,
                    TransactionDate = DateTime.UtcNow,
                    TransactionID = Guid.NewGuid(),
                    TransactionType = TransactionType.Issue,
                };
                await _libraryContextDB.BookTransactions.AddAsync(bookTransaction);
                _libraryContextDB.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Return(Guid id)
        {
            if (id == Guid.Empty || _libraryContextDB.BookTransactions == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var book = _libraryContextDB.Books.FirstOrDefault(b => b.BookID == id);
            if (book != null && book.Quantity > 0)
            {
                BookTransaction bookTransaction = new BookTransaction
                {
                    BookID = id,
                    TransactionDate = DateTime.UtcNow,
                    TransactionID = Guid.NewGuid(),
                    TransactionType = TransactionType.Return,
                };
                await _libraryContextDB.BookTransactions.AddAsync(bookTransaction);
                _libraryContextDB.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
