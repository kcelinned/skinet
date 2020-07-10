using System;
using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    // the connection between the application and database
    // queries the database 
    public class StoreContext : DbContext
    {
        // constructor that creates an instance of context - a session 
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {

        }


        // states these tables in the database 
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }


        // adds the migration configurations 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

