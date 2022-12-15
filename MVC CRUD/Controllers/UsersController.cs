using Microsoft.AspNetCore.Mvc;
using MVC_CRUD.Data;
using MVC_CRUD.Models.Domain;
using MVC_CRUD.Models;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace MVC_CRUD.Controllers
{
    public class UsersController : Controller
    {
        private readonly MVCDbContext dbContext;
        
        public UsersController(MVCDbContext mvcDbContext)
        {
            this.dbContext = mvcDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var users = await dbContext.Users.ToListAsync();
            return View(users);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(UserViewModel userViewModel)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = userViewModel.Name,
                LastName = userViewModel.LastName,
                Email = userViewModel.Email,
                DateOfBirth = userViewModel.DateOfBirth,
                Login = userViewModel.Login,
                Password = userViewModel.Password,
                Role = userViewModel.Role,
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Books");
        }

       
    }
}
