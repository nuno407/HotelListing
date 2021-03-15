using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelListing.Configurations.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data
{
    public class DatabaseContext : IdentityDbContext<ApiUser>
    {
        public DatabaseContext(DbContextOptions options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Loads identity tables
            base.OnModelCreating(modelBuilder);

            // Adding some Roles
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            // Adding some contries
            modelBuilder.ApplyConfiguration(new CountryConfiguration());

            // Adding some Hotels 
            modelBuilder.ApplyConfiguration(new HotelConfiguration());
        }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Hotel> Hotels { get; set; }
    }
}
