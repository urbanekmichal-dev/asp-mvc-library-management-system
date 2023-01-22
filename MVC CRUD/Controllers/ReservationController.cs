using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_CRUD.Data;
using MVC_CRUD.Models;
using MVC_CRUD.Models.Domain;
using System;

namespace MVC_CRUD.Controllers
{
    public class ReservationController : Controller
    {
        private readonly MVCDbContext dbContext;

        public ReservationController(MVCDbContext mvcDbContext)
        {
            this.dbContext = mvcDbContext;

        }
        // GET: HomeController1
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            var reservations = await dbContext.Reservations.Include(n=> n.Book).Include(k=> k.User).Where(r=>r.User.UserName== userName).ToListAsync();
           
            return View(reservations);
        }

        public async Task<IActionResult> IndexAdmin()
        {
            string userName = User.Identity.Name;
            var reservations = await dbContext.Reservations.Include(n => n.Book).Include(k => k.User).ToListAsync();
            return View(reservations);
        }

        // GET: HomeController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController1/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ReturnBook(Guid id)
        {
            var resevation = dbContext.Reservations.Include(n=>n.Book).Where(u => u.ReservationId == id).FirstOrDefault();
            resevation.is_finished = true;
            resevation.Book.Visible = true;

            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> AddReservation(Guid id)
        {
            string userName = User.Identity.Name;
            var user = dbContext.Users.Where(u => u.UserName == userName).FirstOrDefault();
            var book = dbContext.Books.Where(u => u.BookId == id).FirstOrDefault();

            var reservation = new Reservation()
            {
                ReservationId = Guid.NewGuid(),
                User = user,
                Book = book,
                startDate = DateTime.Today,
                endDate = DateTime.Today.AddDays(14),
                daysLeft = 14,
                is_finished = false

            };

            book.Visible = false;
            book.Name = "test1";

            await dbContext.Reservations.AddAsync(reservation);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
