using Contact.Model;
using Microsoft.EntityFrameworkCore;

namespace Contact.Infrastructure;

// DbContext for managing contact user data in the phonebook service

public class ContactUserDbContext(DbContextOptions<ContactUserDbContext> dbContextOptions) 
    : DbContext(dbContextOptions)
{

    public DbSet<ContactUser> contactUsers { get; set; }
}

