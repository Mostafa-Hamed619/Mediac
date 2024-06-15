﻿// <auto-generated />
using System;
using MediacApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MediacApi.Migrations
{
    [DbContext(typeof(MediacDbContext))]
    partial class MediacDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MediacApi.Data.Entities.Blog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("blogDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("blogImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("blogName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("checkFollow")
                        .HasColumnType("bit");

                    b.Property<int>("followers")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("MediacApi.Data.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BlogNumber")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PostName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Refrences")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("firstBody")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("firstHeader")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("postImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("secondBody")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("secondHeader")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("visible")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("BlogNumber");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("MediacApi.Data.Entities.Post", b =>
                {
                    b.HasOne("MediacApi.Data.Entities.Blog", "Blog")
                        .WithMany("posts")
                        .HasForeignKey("BlogNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blog");
                });

            modelBuilder.Entity("MediacApi.Data.Entities.Blog", b =>
                {
                    b.Navigation("posts");
                });
#pragma warning restore 612, 618
        }
    }
}
