﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenVisStreamer.VideoLibrary.Repository.EFC;

#nullable disable

namespace OpenVisStreamer.VideoLibrary.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("OpenVisStreamer.VideoLibrary.Repository.Entities.Video", b =>
                {
                    b.Property<Guid>("VideoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ThumbnailUri")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("UploadDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("VideoUri")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("uploadedByAccoutId")
                        .HasColumnType("char(36)");

                    b.Property<decimal>("videoLength")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("VideoId");

                    b.ToTable("Videos");
                });
#pragma warning restore 612, 618
        }
    }
}
