using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC_CRUD.Models.Domain;

namespace MVC_CRUD.Data
{
    public class MVCDbContext : IdentityDbContext
    {
        private readonly DbContextOptions _options;

        public MVCDbContext(DbContextOptions options) : base(options)
        {
            _options = options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        //var reservation = modelBuilder.Entity<Reservation>();


        //    reservation.HasOne(x => x.User)
        //            .WithOne(x => x.Reservation)
        //            .HasForeignKey<Reservation>(x => x.UserId);

        //    reservation.HasOne(x => x.Book)
        //            .WithOne(x => x.Reservation)
        //            .HasForeignKey<Reservation>(x => x.BookId);


            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Reservation> Reservations{ get; set;}

    }
}
