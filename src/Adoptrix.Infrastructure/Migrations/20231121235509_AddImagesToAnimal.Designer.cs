﻿// <auto-generated />
using System;
using Adoptrix.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Adoptrix.Infrastructure.Migrations
{
    [DbContext(typeof(AdoptrixDbContext))]
    [Migration("20231121235509_AddImagesToAnimal")]
    partial class AddImagesToAnimal
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Adoptrix.Domain.Animal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Species")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("char")
                        .HasColumnName("SpeciesCode");

                    b.HasKey("Id");

                    b.ToTable("Animals");
                });

            modelBuilder.Entity("Adoptrix.Domain.Animal", b =>
                {
                    b.OwnsMany("Adoptrix.Domain.ImageInformation", "Images", b1 =>
                        {
                            b1.Property<Guid>("AnimalId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<string>("ContentType")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("varchar");

                            b1.Property<string>("Description")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("FileName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("varchar");

                            b1.Property<DateTime>("UploadedAt")
                                .ValueGeneratedOnAdd()
                                .HasPrecision(3)
                                .HasColumnType("datetime2(3)")
                                .HasDefaultValueSql("getutcdate()");

                            b1.Property<Guid>("UploadedBy")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("AnimalId", "Id");

                            b1.ToTable("AnimalImages", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("AnimalId");
                        });

                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}