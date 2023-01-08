using Microsoft.AspNetCore.Mvc;
using MVC_CRUD.Data;
using MVC_CRUD.Models.Domain;
using MVC_CRUD.Models;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MVC_CRUD.Controllers
{
    public class UsersController : Controller
    {
        private readonly MVCDbContext dbContext;
        
        public UsersController(MVCDbContext mvcDbContext)
        {
            this.dbContext = mvcDbContext;
        }
        [Authorize(Roles = "ADMIN")]
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
            bool emailExist = dbContext.Users.Any(x=>x.Email==userViewModel.Email);
            //bool loginExist = dbContext.Users.Any(x => x.Login == userViewModel.Login);

            if (emailExist)
            {
                ViewBag.Message = "User with this e-mail address is already registered";
                return View();
            }
            //else if (loginExist)
            //{
            //    ViewBag.Message = "User with this login is already registered";
            //    return View();
            //}
            else
            {
                var user = new User()
                {
                    UserId = Guid.NewGuid(),
                    //Name = userViewModel.Name,
                    //LastName = userViewModel.LastName,
                    //Email = userViewModel.Email,
                    //DateOfBirth = userViewModel.DateOfBirth,
                    //Login = userViewModel.Login,
                    //Password = userViewModel.Password,
                    //Role = "Student",
                };

                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();


                return RedirectToAction("Index", "Books");
            }
            
        }
        [HttpGet]
        public IActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signin(LoginViewModel loginViewModel)
        {
            //if(dbContext.Users.Any(x=>x.Email==loginViewModel.Email && x.Password == loginViewModel.Password))
                if (dbContext.Users.Any(x => x.Email == loginViewModel.Email))

                {
                    return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Wrong email or password! Try again";
                return View();
            }
        }





    }
}
