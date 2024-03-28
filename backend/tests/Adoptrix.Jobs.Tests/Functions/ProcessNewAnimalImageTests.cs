using Adoptrix.Application.Models;
using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Events;
using Adoptrix.Jobs.Functions;
using Adoptrix.Jobs.Tests.Extensions;
using AutoFixture;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Jobs.Tests.Functions;

public class ProcessNewAnimalImageTests
{
    private readonly Mock<ILogger<ProcessNewAnimalImage>> loggerMock = new();
    private readonly Mock<IAnimalImageManager> animalImageManagerMock = new();
    private readonly Mock<IImageProcessor> imageProcessorMock = new();
    private readonly Mock<IAnimalsRepository> animalsRepositoryMock = new();
    private readonly ProcessNewAnimalImage sut;

    public ProcessNewAnimalImageTests()
    {
        sut = new ProcessNewAnimalImage(loggerMock.Object, animalImageManagerMock.Object, imageProcessorMock.Object, animalsRepositoryMock.Object);
    }

    [Fact]
    public async Task Run_WithValidData_Should_Pass()
    {
        // arrange
        var (animal, animalId, imageId) = CreateTestData();
        var originalReadStream = new MemoryStream();
        var eventData = new AnimalImageAddedEvent(animalId, imageId);
        animalImageManagerMock.Setup(manager => manager.GetImageReadStreamAsync(animalId, imageId, It.IsAny<ImageCategory>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(originalReadStream);
        imageProcessorMock.Setup(processor => processor.ProcessOriginalAsync(originalReadStream, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ImageStreamBundle
            {
                ThumbnailWriteStream = new MemoryStream(), PreviewWriteStream = new MemoryStream(), FullSizeWriteStream = new MemoryStream()
            });
        animalsRepositoryMock.Setup(repository => repository.GetAsync(animalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(animal));

        // act
        await sut.Run(eventData);

        // assert
        animalImageManagerMock.Verify(manager => manager.UploadImageAsync(animalId, imageId, It.IsAny<Stream>(), ImageProcessor.OutputContentType,
            ImageCategory.Thumbnail, It.IsAny<CancellationToken>()), Times.Once());
        animalImageManagerMock.Verify(manager => manager.UploadImageAsync(animalId, imageId, It.IsAny<Stream>(), ImageProcessor.OutputContentType,
            ImageCategory.Preview, It.IsAny<CancellationToken>()), Times.Once());
        animalImageManagerMock.Verify(manager => manager.UploadImageAsync(animalId, imageId, It.IsAny<Stream>(), ImageProcessor.OutputContentType,
            ImageCategory.FullSize, It.IsAny<CancellationToken>()), Times.Once());
        loggerMock.VerifyLog($"Uploaded processed images for animal with ID: {animal.Id}", LogLevel.Information, Times.Once());
        animal.Images.First().IsProcessed.Should().BeTrue();
        animalsRepositoryMock.Verify(repository => repository.UpdateAsync(animal, It.IsAny<CancellationToken>()), Times.Once());
    }

    private static (Animal Animal, Guid AnimalId, Guid ImageId) CreateTestData()
    {
        var fixture = new Fixture();
        var animalId = fixture.Create<Guid>();
        var imageId = fixture.Create<Guid>();

        fixture.Customize<Animal>(composer => composer
            .With(animal => animal.Id, animalId)
            .With(animal => animal.DateOfBirth, new DateOnly(DateTime.Now.Year - 2, 1, 1))
            .With(animal => animal.Images, new List<AnimalImage>
            {
                new()
                {
                    Id = imageId,
                    OriginalFileName = fixture.Create<string>(),
                    OriginalContentType = fixture.Create<string>(),
                    UploadedBy = fixture.Create<Guid>(),
                    IsProcessed = false
                }
            }));

        return (fixture.Create<Animal>(), animalId, imageId);
    }
}