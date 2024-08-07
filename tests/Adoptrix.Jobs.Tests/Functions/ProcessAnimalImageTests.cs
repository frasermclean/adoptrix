using Adoptrix.Core;
using Adoptrix.Core.Events;
using Adoptrix.Jobs.Functions;
using Adoptrix.Jobs.Services;
using Adoptrix.Persistence.Services;
using Adoptrix.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq.EntityFrameworkCore;

namespace Adoptrix.Jobs.Tests.Functions;

public class ProcessAnimalImageTests
{
    private readonly Mock<AdoptrixDbContext> dbContextMock;
    private readonly Mock<IImageProcessor> imageProcessorMock = new();
    private readonly Mock<IBlobContainerManager> originalImagesContainerManagerMock = new();
    private readonly Mock<IBlobContainerManager> animalImagesContainerManagerMock = new();
    private readonly ProcessAnimalImage function;

    public ProcessAnimalImageTests()
    {
        dbContextMock =
            new Mock<AdoptrixDbContext>(() => new AdoptrixDbContext(Mock.Of<DbContextOptions<AdoptrixDbContext>>()));
        function = new ProcessAnimalImage(Mock.Of<ILogger<ProcessAnimalImage>>(), dbContextMock.Object,
            imageProcessorMock.Object, originalImagesContainerManagerMock.Object,
            animalImagesContainerManagerMock.Object);
    }

    [Theory, AdoptrixAutoData]
    public async Task ExecuteAsync_WithValidEventData_ShouldPass(Animal animal, ImageStreamBundle bundle)
    {
        // arrange
        var imageId = animal.Images.First().Id;
        var blobName = $"{animal.Id}/image.jpg";
        var data = new AnimalImageAddedEvent(animal.Id, imageId, blobName);
        dbContextMock.Setup<DbSet<Animal>>(dbContext => dbContext.Animals)
            .ReturnsDbSet(new[] {animal});
        // animalsRepositoryMock.Setup(repository => repository.GetByIdAsync(data.AnimalId, It.IsAny<CancellationToken>()))
        //     .ReturnsAsync(animal);
        imageProcessorMock.Setup(processor =>
                processor.ProcessOriginalAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bundle);

        // act
        await function.ExecuteAsync(data);

        // assert
        // animalsRepositoryMock.Verify(repository => repository.GetByIdAsync(animal.Id, It.IsAny<CancellationToken>()),
        //     Times.Once);
        originalImagesContainerManagerMock.Verify(manager =>
            manager.OpenReadStreamAsync(blobName, It.IsAny<CancellationToken>()), Times.Once);
        imageProcessorMock.Verify(processor => processor.ProcessOriginalAsync(It.IsAny<Stream>(),
            It.IsAny<CancellationToken>()), Times.Once);
        animalImagesContainerManagerMock.Verify(manager =>
            manager.UploadBlobAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Exactly(3));
        // animalsRepositoryMock.Verify(repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()),
        //     Times.Once);
        originalImagesContainerManagerMock.Verify(manager =>
            manager.DeleteBlobAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    // [Theory, AdoptrixAutoData]
    // public async Task ExecuteAsync_WithInvalidAnimalId_ShouldThrowException(AnimalImageAddedEvent data)
    // {
    //     // arrange
    //     animalsRepositoryMock.Setup(repository => repository.GetByIdAsync(data.AnimalId, It.IsAny<CancellationToken>()))
    //         .ReturnsAsync(null as Animal);
    //
    //     // act
    //     var act = async () => await function.ExecuteAsync(data);
    //
    //     // assert
    //     await act.Should().ThrowAsync<InvalidOperationException>();
    //     animalsRepositoryMock.Verify(
    //         repository => repository.GetByIdAsync(data.AnimalId, It.IsAny<CancellationToken>()),
    //         Times.Once);
    // }
}
