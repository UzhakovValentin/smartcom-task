using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smartcom.WebApp.Models;
using System;
using System.Threading.Tasks;

namespace Smartcom.WebApp.Database
{
    public static class SeedDatabase
    {
        public static async Task CreateRoles(IServiceProvider provider, IConfiguration configuration)
        {
            var userManager = provider.GetRequiredService<UserManager<Customer>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            var roles = configuration.GetSection("Roles").Get<string[]>();

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            var superUser = new Customer
            {
                Name = configuration.GetSection("SuperUser")["Name"],
                UserName = configuration.GetSection("SuperUser")["Email"],
                Email = configuration.GetSection("SuperUser")["Email"]
            };

            string password = configuration.GetSection("SuperUser")["Password"];
            var user = await userManager.FindByEmailAsync(configuration.GetSection("SuperUser")["Email"]);

            if (user == null)
            {
                var superUserResult = await userManager.CreateAsync(superUser, password);
                if (superUserResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(superUser, "Manager");
                }
            }
        }
    }
}
