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
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
