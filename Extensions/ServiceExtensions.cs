using HotelListing.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<ApiUser>(q =>
                                                        {
                                                            q.Password.RequireDigit = false;
                                                            q.Password.RequiredLength = 4;
                                                            q.Password.RequireLowercase = false;
                                                            q.Password.RequireUppercase = false;
                                                            q.Password.RequiredUniqueChars = 0;
                                                            q.Password.RequireNonAlphanumeric = false;
                                                            q.User.RequireUniqueEmail = true;

                                                        }); 

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();

        }
    }
}
