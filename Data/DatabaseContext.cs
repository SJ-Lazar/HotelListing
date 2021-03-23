using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Data
{
    public class DatabaseContext : IdentityDbContext<ApiUser>
    {
        public DatabaseContext(DbContextOptions options) : base(options) {}
        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Country>()
                            .HasData(new Country
                            {
                                Id = 1,
                                Name ="South Africa",
                                ShortName = "RSA"
                            },
                            new Country
                            {
                                Id = 2,
                                Name = "Canada",
                                ShortName = "CA"
                            },
                            new Country
                            {
                                Id= 3,
                                Name = "United States Of America",
                                ShortName = "USA"
                            }
                );

            builder.Entity<Hotel>()
                           .HasData(new Hotel
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
                           }
               );
        }
    }
}
