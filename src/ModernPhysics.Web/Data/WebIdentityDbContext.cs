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
            builder.Entity<IdentityUser>().Property(p => p.Id).IsUnicode(false);
            builder.Entity<IdentityUser>().Property(p => p.UserName).HasMaxLength(64);
            builder.Entity<IdentityUser>().Property(p => p.NormalizedUserName).HasMaxLength(64);
            builder.Entity<IdentityUser>().Property(p => p.Email).IsUnicode(false);
            builder.Entity<IdentityUser>().Property(p => p.NormalizedEmail).IsUnicode(false);

            builder.Entity<IdentityRole>().Property(p => p.Id).IsUnicode(false);
            builder.Entity<IdentityRole>().Property(p => p.Name).HasMaxLength(64);
            builder.Entity<IdentityRole>().Property(p => p.NormalizedName).HasMaxLength(64);
        }
    }
}
