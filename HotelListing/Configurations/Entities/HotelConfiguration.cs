using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Configurations.Entities
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
                new Hotel { Id = 1, Name = "Pine Cliffs", Address = "Albufeira", CountryId = 1, Rating = 5 },
                new Hotel { Id = 2, Name = "Kilimanjaro", Address = "Sierra Nevada", CountryId = 2, Rating = 3 },
                new Hotel { Id = 3, Name = "Milan HolidayIn", Address = "Milan", CountryId = 3, Rating = 4.69 }
            );
        }
    }
}
