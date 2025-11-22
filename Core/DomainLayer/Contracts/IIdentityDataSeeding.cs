using DomainLayer.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts
{
    public interface IIdentityDataSeeding
    {
        Task SeedRoles(RoleManager<IdentityRole> roleManager);
        Task SeedAdmin(UserManager<ApplicationUser> userManager);
        Task SeedCustomer(UserManager<ApplicationUser> userManager);
        Task SeedAll(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager);
    }
}
