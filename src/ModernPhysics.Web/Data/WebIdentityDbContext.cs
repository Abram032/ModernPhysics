using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ModernPhysics.Web.Data
{
    public class WebIdentityDbContext : IdentityDbContext
    {
        public WebIdentityDbContext(DbContextOptions<WebIdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUser>().Property(p => p.UserName).HasMaxLength(255);
            builder.Entity<IdentityUser>().Property(p => p.NormalizedUserName).HasMaxLength(255);
            builder.Entity<IdentityUser>().Property(p => p.Email).HasMaxLength(255);
            builder.Entity<IdentityUser>().Property(p => p.NormalizedEmail).HasMaxLength(255);

            builder.Entity<IdentityRole>().Property(p => p.Name).HasMaxLength(255);
            builder.Entity<IdentityRole>().Property(p => p.NormalizedName).HasMaxLength(255);
        }
    }
}
