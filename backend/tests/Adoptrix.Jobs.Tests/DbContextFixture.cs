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

        var animal = Animal.Create("Rex", dateOfBirth: new DateOnly(2020, 1, 1));
        var image = AnimalImage.Create(animal.Slug, "Rex in the park", "rex1.jpg", "image/jpeg");
        animal.Images.Add(image);

        DbContext.Animals.Add(animal);
        await DbContext.SaveChangesAsync();

        AnimalSlug = animal.Slug;
        ImageId = image.Id;

    }

    public Task DisposeAsync() => DbContext.DisposeAsync().AsTask();
}
