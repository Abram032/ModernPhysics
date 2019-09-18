using Microsoft.Extensions.Configuration;
using ModernPhysics.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ModernPhysics.Web.Data.Seeders
{
    public class WebAppInitializer : IWebAppInitializer
    {
        public async Task SeedCategoriesAsync(WebAppDbContext context, IConfiguration configuration)
        {
            var category = context.Categories
                .FirstOrDefault(p => p.FriendlyName.Equals("No-category"));

            if(category == null)
            {
                category = new Category
                {
                    Name = "Bez Kategorii",
                    FriendlyName = "No-category",
                    Icon = "fas fa-question",
                    CreatedBy = configuration["AdminUsername"],
                    ModifiedBy = configuration["AdminUsername"]
                };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
            }
        }
    }
}