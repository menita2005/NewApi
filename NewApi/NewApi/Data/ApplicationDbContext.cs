using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NewApi.Models;

namespace NewApi.Data
{
   

        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {

            }

            public DbSet<Supplier> Suppliers { get; set; }
            public DbSet<Customer> Customers { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<OrderItem> OrderItems { get; set; }
            public DbSet<Product> Products { get; set; }
        
    }
}
