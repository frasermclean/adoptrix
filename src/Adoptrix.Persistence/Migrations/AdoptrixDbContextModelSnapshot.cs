﻿// <auto-generated />
using System;
using Adoptrix.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Adoptrix.Persistence.Migrations
{
    [DbContext(typeof(AdoptrixDbContext))]
    partial class AdoptrixDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Adoptrix.Core.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BreedId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(2)
                        .HasColumnType("datetime2(2)")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<Guid>("CreatedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<char>("Sex")
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Slug");

                    b.HasIndex("BreedId");

                    b.ToTable("Animals");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BreedId = 1,
                            DateOfBirth = new DateOnly(2024, 2, 14),
                            Description = "Meet Alberto, a delightful Labrador puppy searching for his forever home. With a golden coat that's as soft as his heart, Alberto's playful spirit is infectious. From chasing butterflies to fetching balls, his days are filled with joy and curiosity. This lovable pup dreams of a family to call his own, where he can share his boundless love and enthusiasm. Could you be the one to open your heart and home to Alberto, making his dreams come true? Adopt this charming ball of fur, and let the adventure of a lifetime begin! 🐾 #AdoptAlberto",
                            IsDeleted = false,
                            Name = "Alberto",
                            Sex = "M",
                            Slug = "alberto-2024-02-14"
                        },
                        new
                        {
                            Id = 2,
                            BreedId = 2,
                            DateOfBirth = new DateOnly(2020, 4, 19),
                            Description = "Meet Barry, a majestic German Shepherd with a heart as loyal as his gaze. At four years old, Barry embodies both strength and gentleness in equal measure. His rich, dark coat gleams in the sunlight as he explores the world with curiosity and confidence. From romping through fields to standing guard with unwavering vigilance, Barry is the epitome of loyalty and companionship. This noble canine seeks a forever home where he can shower his family with unconditional love and protection. Ready to welcome a steadfast friend into your life? Consider adopting Barry, and embark on a journey of trust, devotion, and endless adventure! 🐾 #AdoptBarry",
                            IsDeleted = false,
                            Name = "Barry",
                            Sex = "M",
                            Slug = "barry-2020-04-19"
                        },
                        new
                        {
                            Id = 3,
                            BreedId = 4,
                            DateOfBirth = new DateOnly(2022, 9, 30),
                            Description = "Introducing Ginger, a beautiful, captivating feline with a coat as fiery as her playful spirit. This adorable cat enchants everyone with her graceful moves and amber-colored eyes. From chasing sunbeams to batting at toys, Ginger's days are a whimsical blend of elegance and mischief. This charming kitty yearns for a loving home, where she can curl up on a cozy spot and purr her way into your heart. Are you ready to add a touch of warmth and whimsy to your life? Consider adopting Ginger, and let the purr-fect companionship begin! 🐾 #AdoptGinger",
                            IsDeleted = false,
                            Name = "Ginger",
                            Sex = "F",
                            Slug = "ginger-2022-09-30"
                        },
                        new
                        {
                            Id = 4,
                            BreedId = 5,
                            DateOfBirth = new DateOnly(2017, 4, 11),
                            Description = "Meet Percy, a charming African Grey Parrot with a personality as colorful as his feathers.",
                            IsDeleted = false,
                            Name = "Percy",
                            Sex = "M",
                            Slug = "percy-2017-04-11"
                        });
                });

            modelBuilder.Entity("Adoptrix.Core.Breed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(2)
                        .HasColumnType("datetime2(2)")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<Guid>("CreatedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("SpeciesName")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("SpeciesName");

                    b.ToTable("Breeds");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Labrador Retriever",
                            SpeciesName = "Dog"
                        },
                        new
                        {
                            Id = 2,
                            Name = "German Shepherd",
                            SpeciesName = "Dog"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Golden Retriever",
                            SpeciesName = "Dog"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Domestic Shorthair",
                            SpeciesName = "Cat"
                        },
                        new
                        {
                            Id = 5,
                            Name = "African Grey Parrot",
                            SpeciesName = "Bird"
                        });
                });

            modelBuilder.Entity("Adoptrix.Core.Species", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(2)
                        .HasColumnType("datetime2(2)")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<Guid>("CreatedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.HasKey("Name");

                    b.ToTable("Species");

                    b.HasData(
                        new
                        {
                            Name = "Dog",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = new Guid("00000000-0000-0000-0000-000000000000")
                        },
                        new
                        {
                            Name = "Cat",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = new Guid("00000000-0000-0000-0000-000000000000")
                        },
                        new
                        {
                            Name = "Bird",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = new Guid("00000000-0000-0000-0000-000000000000")
                        });
                });

            modelBuilder.Entity("Adoptrix.Core.Animal", b =>
                {
                    b.HasOne("Adoptrix.Core.Breed", "Breed")
                        .WithMany("Animals")
                        .HasForeignKey("BreedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Adoptrix.Core.AnimalImage", "Images", b1 =>
                        {
                            b1.Property<int>("AnimalId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<DateTime>("CreatedAt")
                                .ValueGeneratedOnAdd()
                                .HasPrecision(2)
                                .HasColumnType("datetime2(2)")
                                .HasDefaultValueSql("getutcdate()");

                            b1.Property<Guid>("CreatedBy")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Description")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<bool>("IsProcessed")
                                .HasColumnType("bit");

                            b1.Property<string>("OriginalContentType")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("OriginalFileName")
                                .IsRequired()
                                .HasMaxLength(512)
                                .HasColumnType("nvarchar(512)");

                            b1.HasKey("AnimalId", "Id");

                            b1.ToTable("AnimalImages", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("AnimalId");
                        });

                    b.Navigation("Breed");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("Adoptrix.Core.Breed", b =>
                {
                    b.HasOne("Adoptrix.Core.Species", "Species")
                        .WithMany("Breeds")
                        .HasForeignKey("SpeciesName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Species");
                });

            modelBuilder.Entity("Adoptrix.Core.Breed", b =>
                {
                    b.Navigation("Animals");
                });

            modelBuilder.Entity("Adoptrix.Core.Species", b =>
                {
                    b.Navigation("Breeds");
                });
#pragma warning restore 612, 618
        }
    }
}
