﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModernPhysics.Web.Data;

namespace ModernPhysics.Web.Data.Migrations.Web
{
    [DbContext(typeof(WebAppDbContext))]
    partial class WebAppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ModernPhysics.Models.Blob", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Type")
                        .HasMaxLength(255);

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("Url")
                        .IsUnique();

                    b.HasIndex("Path", "Name")
                        .IsUnique();

                    b.ToTable("Blobs");
                });

            modelBuilder.Entity("ModernPhysics.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FriendlyName")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("Icon")
                        .HasMaxLength(32);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ModernPhysics.Models.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CategoryId");

                    b.Property<string>("Content")
                        .HasColumnType("MEDIUMTEXT");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("FriendlyUrl")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool>("IsPublished")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<DateTime>("ModifiedAt")
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<int>("Order")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("Shortcut")
                        .HasMaxLength(500);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("FriendlyUrl")
                        .IsUnique();

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("ModernPhysics.Models.Post", b =>
                {
                    b.HasOne("ModernPhysics.Models.Category", "Category")
                        .WithMany("Posts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
