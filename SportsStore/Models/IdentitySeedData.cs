using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SportsStore.Models
{
    public static class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Secret123$";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<AppIdentityDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetService<UserManager<IdentityUser>>();
            var user = await userManager.FindByNameAsync(adminUser); // Database is searched here.

            if(user == null)
            {
                user = new IdentityUser(adminUser);
                user.Email = "admin@example.com";
                user.PhoneNumber = "555-1234";

                await userManager.CreateAsync(user, adminPassword);
            }
        }
    }
}
