using System.Net;
using System.Net.Http.Headers;
using Adoptrix.Core;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Tests.Endpoints.Animals.Images;

public class AddAnimalImagesEndpointTests(App app) : TestBase<App>
{
    private readonly HttpClient httpClient = app.BasicAuthClient;

    [Theory, AdoptrixAutoData]
    public async Task AddAnimalImages_WithValidRequest_ShouldReturnOk(Animal animal)
    {
        // arrange
        using var content = CreateMultipartFormDataContent();
        app.AnimalsRepositoryMock.Setup(repository => repository.GetByIdAsync(animal.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(animal);

        // act
        var message = await httpClient.PostAsync($"api/animals/{animal.Id}/images", content);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        app.AnimalImagesBlobContainerManagerMock.Verify(
            manager => manager.UploadBlobAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Once);
        app.AnimalsRepositoryMock.Verify(repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory, AdoptrixAutoData]
    public async Task AddAnimalImages_WithInvalidAnimalId_ReturnsNotFound(Guid animalId)
    {
        // arrange
        using var content = CreateMultipartFormDataContent();
        app.AnimalsRepositoryMock.Setup(repository => repository.GetByIdAsync(animalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Animal);

        // act
        var message = await httpClient.PostAsync($"api/animals/{animalId}/images", content);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    private static MultipartFormDataContent CreateMultipartFormDataContent(int bufferLength = 1024,
        string fileName = "image.jpg", string contentName = "First image", string contentType = "image/jpeg")
    {
        var buffer = new byte[bufferLength];
        Random.Shared.NextBytes(buffer);

        var content = new StreamContent(new MemoryStream(buffer));
        content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

        return new MultipartFormDataContent
        {
            {
                content, contentName, fileName
            }
        };
    }
}
