using Adoptrix.Application.Models;
using Adoptrix.Application.Services;
using Adoptrix.Domain;
using Adoptrix.Domain.Events;
using Adoptrix.Jobs.Functions;
using Adoptrix.Jobs.Tests.Extensions;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Jobs.Tests.Functions;

public class ProcessNewAnimalImageTests
{
    private readonly Mock<ILogger<ProcessNewAnimalImage>> loggerMock = new();
    private readonly Mock<IAnimalImageManager> animalImageManagerMock = new();
    private readonly Mock<IImageProcessor> imageProcessorMock = new();
    private readonly ProcessNewAnimalImage sut;

    public ProcessNewAnimalImageTests()
    {
        sut = new ProcessNewAnimalImage(loggerMock.Object, animalImageManagerMock.Object, imageProcessorMock.Object);
    }

    [Theory, AutoData]
    public async Task Run_WithValidData_Should_Pass(Guid animalId, Guid imageId)
    {
        // arrange
        var originalReadStream = new MemoryStream();
        var eventData = new AnimalImageAddedEvent(animalId, imageId);
        animalImageManagerMock.Setup(manager => manager.GetImageReadStreamAsync(animalId, imageId, It.IsAny<ImageCategory>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(originalReadStream);
        imageProcessorMock.Setup(processor => processor.ProcessOriginalAsync(originalReadStream, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ImageStreamBundle
            {
                ThumbnailWriteStream = new MemoryStream(),
                PreviewWriteStream = new MemoryStream(),
                FullSizeWriteStream = new MemoryStream()
            });

        // act
        await sut.Run(eventData);

        // assert
        loggerMock.VerifyLog($"Uploaded processed images for animal with ID: {animalId}", LogLevel.Information, Times.Once());
    }
}