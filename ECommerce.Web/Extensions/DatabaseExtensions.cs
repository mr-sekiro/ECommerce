using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Web.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await seeder.DataSeedAsync();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var identitySeeder = scope.ServiceProvider.GetRequiredService<IIdentityDataSeeding>();

            await identitySeeder.SeedAll(userManager, roleManager);
        }
    }
}
