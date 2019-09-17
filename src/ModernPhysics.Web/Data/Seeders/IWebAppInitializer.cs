using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ModernPhysics.Web.Data.Seeders
{
    public interface IWebAppInitializer
    {
        Task SeedCategoriesAsync(WebAppDbContext context, IConfiguration configuration);
    }
}