using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernPhysics.Web.Data
{
    public class WebAppDbContext : DbContext
    {
        public WebAppDbContext(DbContextOptions<WebAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blob> Blobs { get; set; }

        //TODO: Migrate new model and update views
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Post>().HasKey(p => p.Id);
            builder.Entity<Post>().HasIndex(p => p.FriendlyUrl).IsUnique();
            builder.Entity<Post>().Property(p => p.Title).IsRequired().HasMaxLength(255);
            builder.Entity<Post>().Property(p => p.FriendlyUrl).IsRequired().HasMaxLength(255);
            builder.Entity<Post>().Property(p => p.CreatedAt).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Post>().Property(p => p.CreatedBy).IsRequired().HasMaxLength(64);
            builder.Entity<Post>().Property(p => p.ModifiedAt).ValueGeneratedOnAddOrUpdate();
            builder.Entity<Post>().Property(p => p.ModifiedBy).IsRequired().HasMaxLength(64);
            builder.Entity<Post>().Property(p => p.IsPublished).IsRequired().HasDefaultValue(false);
            builder.Entity<Post>().Property(p => p.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Entity<Post>().Property(p => p.ContentType).HasDefaultValue(ContentType.Html).IsRequired();
            builder.Entity<Post>().Property(p => p.Content).HasColumnType("MEDIUMTEXT");
            builder.Entity<Post>().Property(p => p.Shortcut).HasMaxLength(500);
            builder.Entity<Post>().Property(p => p.Order).HasDefaultValue(0);
            builder.Entity<Post>().HasOne(p => p.Category).WithMany(p => p.Posts).IsRequired();
            
            builder.Entity<Category>().HasKey(p => p.Id);
            builder.Entity<Category>().HasIndex(p => p.FriendlyName).IsUnique();
            builder.Entity<Category>().Property(p => p.Name).IsRequired().HasMaxLength(64);
            builder.Entity<Category>().Property(p => p.FriendlyName).IsRequired().HasMaxLength(64);
            builder.Entity<Category>().Property(p => p.Icon).HasMaxLength(32);
            builder.Entity<Category>().Property(p => p.CreatedAt).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Category>().Property(p => p.CreatedBy).IsRequired().HasMaxLength(64);
            builder.Entity<Category>().Property(p => p.ModifiedAt).ValueGeneratedOnAddOrUpdate();
            builder.Entity<Category>().Property(p => p.ModifiedBy).IsRequired().HasMaxLength(64);
            builder.Entity<Category>().HasMany(p => p.Posts).WithOne(p => p.Category);

            builder.Entity<Blob>().HasKey(p => p.Id);
            builder.Entity<Blob>().HasIndex(p => p.Url).IsUnique();
            builder.Entity<Blob>().HasIndex(p => new { p.Path, p.Name }).IsUnique();
            builder.Entity<Blob>().Property(p => p.Url).IsRequired().HasMaxLength(255);
            builder.Entity<Blob>().Property(p => p.Name).IsRequired().HasMaxLength(255);
            builder.Entity<Blob>().Property(p => p.Path).IsRequired().HasMaxLength(255);
            builder.Entity<Blob>().Property(p => p.Description).HasMaxLength(255);
            builder.Entity<Blob>().Property(p => p.Type).HasMaxLength(255);
        }
    }
}
