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
        private readonly IWebHostEnvironment hostEnvironment;
        public BooksController(MVCDbContext mvcDbContext, IWebHostEnvironment hostEnvironment )
        {
            this.dbContext = mvcDbContext;
            this.hostEnvironment = hostEnvironment;
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
            var book =await dbContext.Books.FirstOrDefaultAsync(x => x.BookId == id);

            if(book!=null)
            {
                var viewModel = new UpdateBookModel()
                {
                    Id = book.BookId,
                    Name = book.Name,
                    Author = book.Author,
                  //  PublishingHouse = book.PublishingHouse,
                   // YearOfPublishment = book.YearOfPublishment,
                };
                return await Task.Run(() => View("View",viewModel));
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

            return RedirectToAction("Index");
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
               // book.PublishingHouse = updateBookModel.PublishingHouse;
                //book.YearOfPublishment = updateBookModel.YearOfPublishment;
               await dbContext.SaveChangesAsync();
            }


            return RedirectToAction("Index");
        }


    }
}
