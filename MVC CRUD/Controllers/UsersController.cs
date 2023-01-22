using Microsoft.AspNetCore.Mvc;
using MVC_CRUD.Data;
using MVC_CRUD.Models.Domain;
using MVC_CRUD.Models;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;

namespace MVC_CRUD.Controllers
{
    public class UsersController : Controller
    {
        private readonly MVCDbContext dbContext;
        private readonly IWebHostEnvironment hostEnvironment;

        public UsersController(MVCDbContext mvcDbContext, IWebHostEnvironment hostEnvironment)
        {
            this.dbContext = mvcDbContext;
            this.hostEnvironment = hostEnvironment;
        }
        //[Authorize(Roles = "ADMIN")]
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

        public async Task<ActionResult> Details(Guid id)
        {
            string userName = User.Identity.Name;
            var user = dbContext.Users.Where(u => u.UserName == userName).FirstOrDefault();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserViewModel userModel)
        {
            string userName = User.Identity.Name;
            var user = dbContext.Users.Where(u => u.UserName == userName).FirstOrDefault();

            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.Email = userModel.Email;
            user.Phone = userModel.Phone;
            user.Street = userModel.Street;
            user.State = userModel.State;
            user.ZipCode = userModel.ZipCode;
            user.City = userModel.City;

            if (userModel.Image != null)
            {
                string wwwRootPath = hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(userModel.Image.FileName);
                string extension = Path.GetExtension(userModel.Image.FileName);
                string imageName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image", imageName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await userModel.Image.CopyToAsync(fileStream);
                }
                user.Image = imageName;
            }
            await dbContext.SaveChangesAsync();

            TempData["alertMessage"] = "Updated successfully!";

            return RedirectToAction("Details");
        }

        [HttpGet]
        public async Task<IActionResult> DeactivateUser(String username)
        {
            var user = dbContext.Users.Where(u => u.UserName == username).FirstOrDefault();
            user.LockoutEnabled = true;
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ActivateUser(String username)
        {
            var user = dbContext.Users.Where(u => u.UserName == username).FirstOrDefault();
            user.LockoutEnabled = false;
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
