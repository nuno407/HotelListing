using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Adding some contries
            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, Name = "Portugal", ShortName="PT" },
                new Country { Id = 2, Name = "Spain", ShortName = "ES" },
                new Country { Id = 3, Name = "Italy", ShortName = "IT" }
            );

            // Adding some Hotels 
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel { Id = 1, Name = "Pine Cliffs", Address = "Albufeira", CountryId = 1, Rating = 5 },
                new Hotel { Id = 2, Name = "Kilimanjaro", Address = "Sierra Nevada", CountryId = 2, Rating = 3 },
                new Hotel { Id = 3, Name = "Milan HolidayIn", Address = "Milan", CountryId = 3, Rating = 4.69 }
            );
        }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Hotel> Hotels { get; set; }
    }
}
