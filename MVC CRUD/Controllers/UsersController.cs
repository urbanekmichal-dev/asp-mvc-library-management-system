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
    [Authorize]
    public class UsersController : Controller
    {
        private readonly MVCDbContext dbContext;
        private readonly IWebHostEnvironment hostEnvironment;

        public UsersController(MVCDbContext mvcDbContext, IWebHostEnvironment hostEnvironment)
        {
            this.dbContext = mvcDbContext;
            this.hostEnvironment = hostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var users = await dbContext.Users.ToListAsync();
            return View(users);
        }
        public async Task<ActionResult> Details(Guid id)
        {
            string userName = User.Identity.Name;
            var user = dbContext.Users.Where(u => u.UserName == userName).FirstOrDefault();
            return View(user);
        }
        public async Task<ActionResult> DetailsAdmin(string username)
        {
            var user = dbContext.Users.Where(u => u.UserName == username).FirstOrDefault();
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

        [HttpGet]
        public async Task<IActionResult> Delete(String username)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (user != null)
            {
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

    }
}
