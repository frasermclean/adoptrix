using Adoptrix.Core;
using Adoptrix.Persistence.Services;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Jobs.Tests;

public class DbContextFixture : IAsyncLifetime
{
    public AdoptrixDbContext DbContext { get; }

    public string AnimalSlug { get; } = "rex-the-dog";
    public Guid ImageId { get; } = Guid.NewGuid();

    public DbContextFixture()
    {
        var options = new DbContextOptionsBuilder<AdoptrixDbContext>()
            .UseInMemoryDatabase("Adoptrix")
            .Options;

        DbContext = new AdoptrixDbContext(options);
    }

    public async Task InitializeAsync()
    {
        await DbContext.Database.EnsureCreatedAsync();

        DbContext.Animals.Add(new Animal
        {
            Id = Guid.NewGuid(),
            Name = "Rex",
            Breed = new Breed
            {
                Id = 1,
                Name = "Jack Russell Terrier",
                Species = new Species
                {
                    Id = 1,
                    Name = "Dog"
                },

            },
            Sex = Sex.Male,
            DateOfBirth = new DateOnly(2020, 1, 1),
            Slug = AnimalSlug,
            Images =
            [
                new AnimalImage
                {
                    Id = ImageId,
                    IsProcessed = false,
                    AnimalSlug = AnimalSlug,
                    OriginalFileName = "rex1.jpg",
                    OriginalContentType = "image/jpeg"
                }
            ]
        });

        await DbContext.SaveChangesAsync();
    }

    public Task DisposeAsync() => DbContext.DisposeAsync().AsTask();
}
