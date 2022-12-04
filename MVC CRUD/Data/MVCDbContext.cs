using Microsoft.EntityFrameworkCore;
using MVC_CRUD.Models.Domain;

namespace MVC_CRUD.Data
{
    public class MVCDbContext : DbContext
    {
        public MVCDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}
