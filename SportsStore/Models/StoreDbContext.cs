using System;
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { } // send DbContextOptions to base from constructor. Dbcontext provides informaion such as connection string, db provider etc.

        public DbSet<Product> Products { get; set; } // To access the Db table
        public DbSet<Order> Orders { get; set; }
    }
}
