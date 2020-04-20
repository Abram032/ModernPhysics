﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModernPhysics.Web.Data;

namespace ModernPhysics.Web.Data.Migrations.Web
{
    [DbContext(typeof(WebAppDbContext))]
    [Migration("20200420184100_RemovedTextRequirementOnQA")]
    partial class RemovedTextRequirementOnQA
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ModernPhysics.Models.Answer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsCorrect")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<Guid?>("QuestionId");

                    b.Property<string>("Text")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("ModernPhysics.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("FriendlyName")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("Icon")
                        .HasMaxLength(64);

                    b.Property<DateTime>("ModifiedAt")
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("FriendlyName")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ModernPhysics.Models.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CategoryId");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<int>("ContentType")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

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

            modelBuilder.Entity("ModernPhysics.Models.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("QuizId");

                    b.Property<string>("Text")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("ModernPhysics.Models.Quiz", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

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

                    b.Property<Guid?>("PostId");

                    b.Property<int>("TimesSolved")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<int>("TimesSolvedCorrectly")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("FriendlyUrl")
                        .IsUnique();

                    b.HasIndex("PostId")
                        .IsUnique();

                    b.ToTable("Quizzes");
                });

            modelBuilder.Entity("ModernPhysics.Models.Answer", b =>
                {
                    b.HasOne("ModernPhysics.Models.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId");
                });

            modelBuilder.Entity("ModernPhysics.Models.Post", b =>
                {
                    b.HasOne("ModernPhysics.Models.Category", "Category")
                        .WithMany("Posts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ModernPhysics.Models.Question", b =>
                {
                    b.HasOne("ModernPhysics.Models.Quiz", "Quiz")
                        .WithMany("Questions")
                        .HasForeignKey("QuizId");
                });

            modelBuilder.Entity("ModernPhysics.Models.Quiz", b =>
                {
                    b.HasOne("ModernPhysics.Models.Post", "Post")
                        .WithOne("Quiz")
                        .HasForeignKey("ModernPhysics.Models.Quiz", "PostId");
                });
#pragma warning restore 612, 618
        }
    }
}
