using Contact_Prj.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Contact_Prj.Context
{
    public class ContactsDbContext : DbContext
    {
        public ContactsDbContext(DbContextOptions<ContactsDbContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }

    }
}
