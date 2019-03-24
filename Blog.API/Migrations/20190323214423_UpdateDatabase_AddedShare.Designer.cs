﻿// <auto-generated />
using System.Collections.Generic;
using Blog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Blog.API.Migrations
{
    [DbContext(typeof(BlogContext))]
    [Migration("20190323214423_UpdateDatabase_AddedShare")]
    partial class UpdateDatabase_AddedShare
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Blog.Model.Entities.Like", b =>
                {
                    b.Property<string>("StoryId");

                    b.Property<string>("UserId");

                    b.HasKey("StoryId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("Like");
                });

            modelBuilder.Entity("Blog.Model.Entities.Share", b =>
                {
                    b.Property<string>("StoryId");

                    b.Property<string>("UserId");

                    b.HasKey("StoryId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("Share");
                });

            modelBuilder.Entity("Blog.Model.Entities.Story", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<long>("CreationTime");

                    b.Property<bool>("Draft");

                    b.Property<long>("LastEditTime");

                    b.Property<string>("OwnerId")
                        .IsRequired();

                    b.Property<long>("PublishTime");

                    b.Property<List<string>>("Tags");

                    b.Property<string>("Title")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Story");
                });

            modelBuilder.Entity("Blog.Model.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<string>("Password");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Blog.Model.Entities.Like", b =>
                {
                    b.HasOne("Blog.Model.Entities.Story", "Story")
                        .WithMany("Likes")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Blog.Model.Entities.User", "User")
                        .WithMany("Likes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Blog.Model.Entities.Share", b =>
                {
                    b.HasOne("Blog.Model.Entities.Story", "Story")
                        .WithMany("Shares")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Blog.Model.Entities.User", "User")
                        .WithMany("Shares")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Blog.Model.Entities.Story", b =>
                {
                    b.HasOne("Blog.Model.Entities.User", "Owner")
                        .WithMany("Stories")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.SetNull);
                });
#pragma warning restore 612, 618
        }
    }
}
