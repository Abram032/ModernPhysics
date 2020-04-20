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
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

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
            builder.Entity<Post>().Property(p => p.Content).HasColumnType("TEXT");
            builder.Entity<Post>().Property(p => p.Shortcut).HasMaxLength(500);
            builder.Entity<Post>().HasOne(p => p.Category).WithMany(p => p.Posts).IsRequired();
            builder.Entity<Post>().HasOne(p => p.Quiz).WithOne(p => p.Post).HasForeignKey<Quiz>(p => p.PostId);
            
            builder.Entity<Category>().HasKey(p => p.Id);
            builder.Entity<Category>().HasIndex(p => p.FriendlyName).IsUnique();
            builder.Entity<Category>().Property(p => p.Name).IsRequired().HasMaxLength(64);
            builder.Entity<Category>().Property(p => p.FriendlyName).IsRequired().HasMaxLength(64);
            builder.Entity<Category>().Property(p => p.Icon).HasMaxLength(64);
            builder.Entity<Category>().Property(p => p.CreatedAt).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Category>().Property(p => p.CreatedBy).IsRequired().HasMaxLength(64);
            builder.Entity<Category>().Property(p => p.ModifiedAt).ValueGeneratedOnAddOrUpdate();
            builder.Entity<Category>().Property(p => p.ModifiedBy).IsRequired().HasMaxLength(64);
            builder.Entity<Category>().HasMany(p => p.Posts).WithOne(p => p.Category);

            builder.Entity<Quiz>().HasKey(p => p.Id);
            builder.Entity<Quiz>().HasIndex(p => p.FriendlyUrl).IsUnique();
            builder.Entity<Quiz>().Property(p => p.Title).IsRequired().HasMaxLength(255);
            builder.Entity<Quiz>().Property(p => p.FriendlyUrl).IsRequired().HasMaxLength(255);
            builder.Entity<Quiz>().Property(p => p.CreatedAt).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Quiz>().Property(p => p.CreatedBy).IsRequired().HasMaxLength(64);
            builder.Entity<Quiz>().Property(p => p.ModifiedAt).ValueGeneratedOnAddOrUpdate();
            builder.Entity<Quiz>().Property(p => p.ModifiedBy).IsRequired().HasMaxLength(64);
            builder.Entity<Quiz>().Property(p => p.IsPublished).IsRequired().HasDefaultValue(false);
            builder.Entity<Quiz>().Property(p => p.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Entity<Quiz>().Property(p => p.TimesSolved).IsRequired().HasDefaultValue(0);
            builder.Entity<Quiz>().Property(p => p.TimesSolvedCorrectly).IsRequired().HasDefaultValue(0);
            builder.Entity<Quiz>().HasMany(p => p.Questions).WithOne(p => p.Quiz);

            builder.Entity<Question>().HasKey(p => p.Id);
            builder.Entity<Question>().Property(p => p.Text).HasMaxLength(255);
            builder.Entity<Question>().HasOne(p => p.Quiz).WithMany(p => p.Questions);
            builder.Entity<Question>().HasMany(p => p.Answers).WithOne(p => p.Question);

            builder.Entity<Answer>().HasKey(p => p.Id);
            builder.Entity<Answer>().Property(p => p.Text).HasMaxLength(255);
            builder.Entity<Answer>().Property(p => p.IsCorrect).IsRequired().HasDefaultValue(false);
            builder.Entity<Answer>().HasOne(p => p.Question).WithMany(p => p.Answers);
        }
    }
}
