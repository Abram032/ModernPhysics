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

        public DbSet<Page> Pages { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PageTag> PageTags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blob> Blobs { get; set; }

        //TODO: Migrate new model and update views
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Page>().HasKey(p => p.Id);
            builder.Entity<Page>().HasIndex(p => p.FriendlyUrl).IsUnique();
            builder.Entity<Page>().Property(p => p.Title).IsRequired().HasMaxLength(255);
            builder.Entity<Page>().Property(p => p.FriendlyUrl).IsRequired().HasMaxLength(255);
            builder.Entity<Page>().Property(p => p.CreatedAt).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Page>().Property(p => p.CreatedBy).IsRequired().HasMaxLength(64);
            builder.Entity<Page>().Property(p => p.ModifiedAt).ValueGeneratedOnUpdate();
            builder.Entity<Page>().Property(p => p.ModifiedBy).HasMaxLength(64);
            builder.Entity<Page>().Property(p => p.IsPublished).IsRequired().HasDefaultValue(false);
            builder.Entity<Page>().Property(p => p.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Entity<Page>().Property(p => p.Content).HasColumnType("LONGTEXT");
            builder.Entity<Page>().Property(p => p.Order).ValueGeneratedOnAdd();
            builder.Entity<Page>().HasOne(p => p.Category).WithMany(p => p.Pages).IsRequired();
            builder.Entity<Page>().HasMany(p => p.PageTags).WithOne(p => p.Page).HasForeignKey(p => p.TagId);

            builder.Entity<Tag>().HasKey(p => p.Id);
            builder.Entity<Tag>().HasIndex(p => p.Name).IsUnique();
            builder.Entity<Tag>().Property(p => p.Name).HasMaxLength(64).IsRequired();
            builder.Entity<Tag>().HasMany(p => p.PageTags).WithOne(p => p.Tag).HasForeignKey(p => p.PageId);

            builder.Entity<PageTag>().HasKey(p => new { p.PageId, p.TagId });
            builder.Entity<PageTag>().HasOne(p => p.Page).WithMany(p => p.PageTags).HasForeignKey(p => p.PageId);
            builder.Entity<PageTag>().HasOne(p => p.Tag).WithMany(p => p.PageTags).HasForeignKey(p => p.TagId);

            builder.Entity<Category>().HasKey(p => p.Id);
            builder.Entity<Category>().HasIndex(p => p.Name).IsUnique();
            builder.Entity<Category>().Property(p => p.Name).IsRequired().HasMaxLength(64);
            builder.Entity<Category>().HasMany(p => p.Pages).WithOne(p => p.Category);

            builder.Entity<Blob>().HasKey(p => p.Id);
            builder.Entity<Blob>().HasIndex(p => p.Url).IsUnique();
            builder.Entity<Blob>().HasIndex(p => new { p.Path, p.Name }).IsUnique();
            builder.Entity<Blob>().Property(p => p.Url).IsRequired().HasMaxLength(2048);
            builder.Entity<Blob>().Property(p => p.Name).IsRequired().HasMaxLength(255);
            builder.Entity<Blob>().Property(p => p.Path).IsRequired().HasMaxLength(255);
            builder.Entity<Blob>().Property(p => p.Description).HasMaxLength(255);
            builder.Entity<Blob>().Property(p => p.Type).HasMaxLength(255);
        }
    }
}
