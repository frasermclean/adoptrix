using Adoptrix.Core.Events;
using Adoptrix.Jobs.Functions;
using Adoptrix.Jobs.Services;
using Adoptrix.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Jobs.Tests.Functions;

public class ProcessAnimalImageTests : IClassFixture<DbContextFixture>
{
    private readonly Mock<ILogger<ProcessAnimalImage>> loggerMock = new();
    private readonly Mock<IImageProcessor> imageProcessorMock = new();
    private readonly Mock<IBlobContainerManager> originalImagesContainerManagerMock = new();
    private readonly Mock<IBlobContainerManager> animalImagesContainerManagerMock = new();
    private readonly ProcessAnimalImage function;
    private readonly AdoptrixDbContext dbContext;

    private readonly AnimalImageAddedEvent eventData;


    public ProcessAnimalImageTests(DbContextFixture fixture)
    {
        function = new ProcessAnimalImage(
            loggerMock.Object,
            fixture.DbContext,
            imageProcessorMock.Object,
            originalImagesContainerManagerMock.Object,
            animalImagesContainerManagerMock.Object);

        dbContext = fixture.DbContext;
        eventData = new AnimalImageAddedEvent(fixture.AnimalSlug, fixture.ImageId, "rex1.jpg");
    }

    [Fact]
    public async Task ExecuteAsync_WithValidEventData_ShouldPass()
    {
        // arrange
        imageProcessorMock.Setup(processor =>
                processor.ProcessOriginalAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ImageStreamBundle
            {
                ThumbnailWriteStream = new MemoryStream(),
                PreviewWriteStream = new MemoryStream(),
                FullSizeWriteStream = new MemoryStream()
            });

        originalImagesContainerManagerMock
            .Setup(manager => manager.OpenReadStreamAsync(eventData.BlobName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MemoryStream());

        // act
        await function.ExecuteAsync(eventData);

        // assert
        var animal = await dbContext.Animals.FirstOrDefaultAsync(a => a.Slug == eventData.AnimalSlug);
        animal.Should().NotBeNull();
        animal!.Images.Should().ContainSingle(image => image.IsProcessed);
    }

    [Fact]
    public async Task ExecuteAsync_WithUnknownAnimalSlug_ShouldFail()
    {
        // arrange
        var data = new AnimalImageAddedEvent("unknown-slug", Guid.NewGuid(), "unknown.jpg");

        // act
        var act = async () => await function.ExecuteAsync(data);

        // assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
