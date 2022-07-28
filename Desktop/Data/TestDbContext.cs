using ApiServices;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Desktop.Data
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ContactsTestDb;Integrated Security=True");
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
