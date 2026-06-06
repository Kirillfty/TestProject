using Microsoft.EntityFrameworkCore;
using test.Database.Entitty;

namespace test.Database
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Contact> Contacts { get; set; }
    }
}