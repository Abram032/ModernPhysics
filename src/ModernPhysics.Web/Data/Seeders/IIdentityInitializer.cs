using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ModernPhysics.Web.Data.Seeders
{
    public interface IIdentityInitializer
    {
         Task SeedUsersAsync(UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, IConfiguration configuration);
    }
}