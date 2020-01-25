using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Web.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using ModernPhysics.Web.Data.Seeders;
using FluentValidation.AspNetCore;
using ModernPhysics.Web.Areas.Admin.Pages.Manage.Categories;
using FluentValidation;
using ModernPhysics.Web.Areas.Admin.Pages.Manage.Posts;
using ModernPhysics.Web.Utils;

namespace ModernPhysics.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddTransient<IWebAppInitializer, WebAppInitializer>();
            services.AddTransient<IIdentityInitializer, IdentityInitializer>();

            services.AddTransient<IValidator<InputCategoryModel>, InputCategoryValidator>();
            services.AddTransient<IValidator<InputPostModel>, InputPostValidator>();

            services.AddTransient<ICharacterParser, CharacterParser>();

            services.AddDbContext<WebIdentityDbContext>(options =>
            {
                options.UseMySql(Configuration["SqlConnectionString"]);
            });
            services.AddDbContext<WebAppDbContext>(options =>
            {
                options.UseMySql(Configuration["SqlConnectionString"]);
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<WebIdentityDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = false;
            });

            services.AddAuthorization(options => 
            {
                options.AddPolicy("IsAdmin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("IsAdminOrModerator", policy => policy.RequireRole("Admin", "Moderator"));
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeAreaFolder("Admin", "/Manage");
                })
                .AddFluentValidation();

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.LoginPath = $"/Admin/Login";
                options.LogoutPath = $"/Admin/Logout";
                options.AccessDeniedPath = $"/Admin/AccessDenied";
                options.SlidingExpiration = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            WebAppDbContext context)
        {         
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "content")),
                RequestPath = "/content"
            });

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
