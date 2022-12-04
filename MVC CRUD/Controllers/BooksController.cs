using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_CRUD.Data;
using MVC_CRUD.Models;
using MVC_CRUD.Models.Domain;

namespace MVC_CRUD.Controllers
{
    public class BooksController : Controller

    {
        private readonly MVCDbContext dbContext;
        public BooksController(MVCDbContext mvcDbContext)
        {
            this.dbContext = mvcDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var books = await dbContext.Books.ToListAsync();
            return View(books);
        }
        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var book =await dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);

            if(book!=null)
            {
                var viewModel = new UpdateBookModel()
                {
                    Id = book.Id,
                    Name = book.Name,
                    Author = book.Author,
                    PublishingHouse = book.PublishingHouse,
                    YearOfPublishment = book.YearOfPublishment,
                };
                return await Task.Run(() => View("View",viewModel));
            }


            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var book = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book != null)
            {
                dbContext.Books.Remove(book);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> Add(BookViewModel addBookRequest)
        {
            var book = new Book()
            {
                Id = Guid.NewGuid(),
                Name = addBookRequest.Name,
                Author = addBookRequest.Author,
                PublishingHouse = addBookRequest.PublishingHouse,
                YearOfPublishment = addBookRequest.YearOfPublishment,
            };

            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> View(UpdateBookModel updateBookModel)
        {
            var book = await dbContext.Books.FindAsync(updateBookModel.Id);

            if (book != null)
            {
                book.Name = updateBookModel.Name;
                book.Author = updateBookModel.Author;
                book.PublishingHouse = updateBookModel.PublishingHouse;
                book.YearOfPublishment = updateBookModel.YearOfPublishment;
               await dbContext.SaveChangesAsync();
            }


            return RedirectToAction("Index");
        }
    }
}
