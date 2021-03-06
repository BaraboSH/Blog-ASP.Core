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
    [Migration("20190317213826_ChangeMaxLength2")]
    partial class ChangeMaxLength2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

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

            modelBuilder.Entity("Blog.Model.Entities.Story", b =>
                {
                    b.HasOne("Blog.Model.Entities.User", "Owner")
                        .WithMany("Stories")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
