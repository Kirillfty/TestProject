using Microsoft.EntityFrameworkCore;
using Test.Backend.Database.Entitty;

namespace Test.Backend.Database
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Contact> Contacts { get; set; }
    }
}