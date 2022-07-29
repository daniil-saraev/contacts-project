using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsDatabaseAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
