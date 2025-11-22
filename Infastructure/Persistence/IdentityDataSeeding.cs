using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class IdentityDataSeeding : IIdentityDataSeeding
    {
        public async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Super Admin", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public async Task SeedAdmin(UserManager<ApplicationUser> userManager)
        {
            string adminEmail = "admin@store.com";
            string superAdminEmail = "superadmin@store.com";

            // Admin
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    DisplayName = "Default Admin",
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                {
                    var createdAdmin = await userManager.FindByEmailAsync(adminEmail);
                    await userManager.AddToRoleAsync(createdAdmin!, "Admin");
                }
            }

            // Super Admin
            if (await userManager.FindByEmailAsync(superAdminEmail) == null)
            {
                var superAdmin = new ApplicationUser
                {
                    DisplayName = "Super Admin",
                    UserName = "superadmin",
                    Email = superAdminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(superAdmin, "SuperAdmin@123");
                if (result.Succeeded)
                {
                    var createdSuperAdmin = await userManager.FindByEmailAsync(superAdminEmail);
                    await userManager.AddToRoleAsync(createdSuperAdmin!, "Super Admin");
                }
            }
        }

        public async Task SeedCustomer(UserManager<ApplicationUser> userManager)
        {
            string customerEmail = "customer@store.com";

            if (await userManager.FindByEmailAsync(customerEmail) == null)
            {
                var customer = new ApplicationUser
                {
                    DisplayName = "Store Customer",
                    UserName = "customer",
                    Email = customerEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(customer, "Customer@123");
                if (result.Succeeded)
                {
                    var createdCustomer = await userManager.FindByEmailAsync(customerEmail);
                    await userManager.AddToRoleAsync(createdCustomer!, "Customer");
                }
            }
        }

        public async Task SeedAll(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedAdmin(userManager);
            await SeedCustomer(userManager);
        }
    }
}
