using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Data.Context;

internal class ContactsDbContext : DbContext
{
    public ContactsDbContext(DbContextOptions<ContactsDbContext> options) : base(options)
    {

    }

    public DbSet<Contact> Contacts { get; set; }
}
