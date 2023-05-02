using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Smart_Invoice.Models;

namespace Smart_Invoice.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ContactPerson> Contacts { get; set; }
        public DbSet<UtilityInvoice> UtilityInvoices { get; set; }
        public DbSet<Company> Companies { get; set; }

    }
}