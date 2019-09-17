using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModernPhysics.Web.Data;
using ModernPhysics.Web.Data.Seeders;

namespace ModernPhysics.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            MigrateAndSeedDatabase(host).Wait();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static async Task MigrateAndSeedDatabase(IWebHost host)
        {
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "content"));

            using(var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var webAppContext = scope.ServiceProvider.GetService<WebAppDbContext>();
                var identityContext = scope.ServiceProvider.GetService<WebIdentityDbContext>();

                await webAppContext.Database.MigrateAsync();
                await identityContext.Database.MigrateAsync();

                var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var webAppInit = scope.ServiceProvider.GetService<IWebAppInitializer>();
                var identityInit = scope.ServiceProvider.GetService<IIdentityInitializer>();
                var configuration = scope.ServiceProvider.GetService<IConfiguration>();

                await webAppInit.SeedCategoriesAsync(webAppContext, configuration);
                await identityInit.SeedUsersAsync(userManager, roleManager, configuration);
            }
        }
    }
}