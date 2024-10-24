using Adoptrix.Core;
using Adoptrix.Persistence.Services;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Jobs.Tests;

public class DbContextFixture : IAsyncLifetime
{
    public AdoptrixDbContext DbContext { get; }

    public string AnimalSlug { get; private set; } = string.Empty;
    public Guid ImageId { get; private set; }

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

        var breed = await DbContext.Breeds.FirstAsync(b => b.Id == 1);
        var animal = new Animal("Rex", new DateOnly(2020, 1, 1))
        {
            Breed = breed,
            Sex = Sex.Male
        };

        var image = new AnimalImage
        {
            Id = Guid.NewGuid(),
            AnimalSlug = animal.Slug,
            Description = "Rex in the park",
            OriginalFileName = "rex1.jpg",
            OriginalContentType = "image/jpeg"
        };
        animal.Images.Add(image);

        DbContext.Animals.Add(animal);
        await DbContext.SaveChangesAsync();

        AnimalSlug = animal.Slug;
        ImageId = image.Id;
    }

    public Task DisposeAsync() => DbContext.DisposeAsync().AsTask();
}
