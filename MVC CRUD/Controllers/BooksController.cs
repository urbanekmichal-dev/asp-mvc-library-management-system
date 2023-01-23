using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_CRUD.Data;
using MVC_CRUD.Models;
using MVC_CRUD.Models.Domain;


namespace MVC_CRUD.Controllers
{
    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
    [Authorize]
    public class BooksController : Controller

    {
        private readonly MVCDbContext dbContext;
        private readonly IWebHostEnvironment hostEnvironment;
        public BooksController(MVCDbContext mvcDbContext, IWebHostEnvironment hostEnvironment)
        {
            this.dbContext = mvcDbContext;
            this.hostEnvironment = hostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var books = await dbContext.Books.Where(b => b.Visible == true).ToListAsync();
            foreach (Book b in books)
            {
                b.Description = b.Description.Truncate(150);
            }
            return View(books);
        }


        [HttpGet]
        public async Task<IActionResult> IndexAdmin()
        {
            var books = await dbContext.Books.ToListAsync();
            foreach (Book b in books)
            {
                b.Description = b.Description.Truncate(150);
            }
            return View(books);
        }
        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var book = await dbContext.Books.FirstOrDefaultAsync(x => x.BookId == id);

            if (book != null)
            {
                return await Task.Run(() => View("View", book));
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var book = await dbContext.Books.FirstOrDefaultAsync(x => x.BookId == id);

            if (book != null)
            {
                dbContext.Books.Remove(book);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("IndexAdmin");
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(BookViewModel addBookRequest)
        {
            var book = new Book()
            {
                BookId = Guid.NewGuid(),
                Name = addBookRequest.Name,
                Author = addBookRequest.Author,
                Publisher = addBookRequest.Publisher,
                Published = addBookRequest.Published.Date,
                Category = addBookRequest.Category,
                Description = addBookRequest.Description,
                Visible = addBookRequest.Visible
            };

            string wwwRootPath = hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(addBookRequest.Image.FileName);
            string extension = Path.GetExtension(addBookRequest.Image.FileName);
            string imageName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(wwwRootPath + "/Image", imageName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await addBookRequest.Image.CopyToAsync(fileStream);
            }
            book.Image = imageName;

            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("IndexAdmin");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var book = await dbContext.Books.FirstOrDefaultAsync(x => x.BookId == id);
            if (book != null)
            {
                return await Task.Run(() => View("Edit", book));
            }

            return RedirectToAction("IndexAdmin");
        }
        [HttpPost]
        public async Task<IActionResult> Update(Book addBookRequest)
        {
            var book = await dbContext.Books.FirstOrDefaultAsync(x => x.BookId == addBookRequest.BookId);
            if (book != null)
            {

                book.Name = addBookRequest.Name;
                book.Author = addBookRequest.Author;
                book.Published = addBookRequest.Published;
                book.Publisher = addBookRequest.Publisher;
                book.Category = addBookRequest.Category;
                book.Description = addBookRequest.Description;
                book.Visible = addBookRequest.Visible;

                await dbContext.SaveChangesAsync();
            }


            return RedirectToAction("IndexAdmin");
        }
        [HttpGet]
        public async Task<IActionResult> ChangeVisibility(Guid id)
        {
            var book = await dbContext.Books.FirstOrDefaultAsync(x => x.BookId == id);
            if (book != null)
            {
                if (book.Visible == true)
                {
                    book.Visible = false;
                }
                else
                {
                    book.Visible = true;
                }
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("IndexAdmin");
        }

    }
}
