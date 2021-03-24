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
            #region HotelSeedData
            builder.HasData(
                       new Hotel
                       {
                           Id = 1,
                           Name = "Sun City Hotel",
                           Address = "Sun Lane 245",
                           CountryId = 1,
                           Rating = 5
                       },
                       new Hotel
                       {
                           Id = 2,
                           Name = "Moon City Hotel",
                           Address = "Moon Lane 105",
                           CountryId = 2,
                           Rating = 4
                       },
                       new Hotel
                       {
                           Id = 3,
                           Name = "Mars City Hotel",
                           Address = "Mars Lane 5",
                           CountryId = 3,
                           Rating = 3.5
                       }); 
            #endregion
        }
    }
}
