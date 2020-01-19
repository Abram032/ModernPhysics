using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernPhysics.Web.Data.Seeders
{
    public class IdentityInitializer : IIdentityInitializer
    {
        public async Task SeedUsersAsync(UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            if(await roleManager.RoleExistsAsync("Admin") == false)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                };

                await roleManager.CreateAsync(role);
            }

            if(await roleManager.RoleExistsAsync("Moderator") == false)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Moderator",
                    NormalizedName = "Moderator".ToUpper()
                };

                await roleManager.CreateAsync(role);
            }

            if (await userManager.FindByNameAsync(configuration["AdminUsername"]) == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = configuration["AdminUsername"]                  
                };

                var result = await userManager.CreateAsync(user, configuration["AdminPassword"]);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
