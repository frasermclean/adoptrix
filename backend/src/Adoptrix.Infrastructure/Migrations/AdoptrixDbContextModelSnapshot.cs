﻿// <auto-generated />
using System;
using Adoptrix.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Adoptrix.Infrastructure.Migrations
{
    [DbContext(typeof(AdoptrixDbContext))]
    partial class AdoptrixDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Adoptrix.Domain.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<int>("SpeciesId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SpeciesId");

                    b.ToTable("Animals");
                });

            modelBuilder.Entity("Adoptrix.Domain.Species", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Species");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Dog"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Cat"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Horse"
                        });
                });

            modelBuilder.Entity("Adoptrix.Domain.Animal", b =>
                {
                    b.HasOne("Adoptrix.Domain.Species", "Species")
                        .WithMany()
                        .HasForeignKey("SpeciesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Adoptrix.Domain.ImageInformation", "Images", b1 =>
                        {
                            b1.Property<int>("AnimalId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<string>("Description")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("FileName")
                                .IsRequired()
                                .HasMaxLength(40)
                                .HasColumnType("varchar");

                            b1.Property<string>("OriginalFileName")
                                .IsRequired()
                                .HasMaxLength(512)
                                .HasColumnType("nvarchar");

                            b1.Property<DateTime>("UploadedAt")
                                .ValueGeneratedOnAdd()
                                .HasPrecision(2)
                                .HasColumnType("datetime2(2)")
                                .HasDefaultValueSql("getutcdate()");

                            b1.Property<Guid?>("UploadedBy")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("AnimalId", "Id");

                            b1.ToTable("AnimalImages", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("AnimalId");
                        });

                    b.Navigation("Images");

                    b.Navigation("Species");
                });
#pragma warning restore 612, 618
        }
    }
}