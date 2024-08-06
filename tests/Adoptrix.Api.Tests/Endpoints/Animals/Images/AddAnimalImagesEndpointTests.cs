using System.Net;
using System.Net.Http.Headers;
using Adoptrix.Core;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Animals.Images;

[Collection(nameof(ApiCollection))]
[Trait("Category", "Integration")]
public class AddAnimalImagesEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.AdminClient;

    [Fact]
    public async Task AddAnimalImages_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        const int animalId = 1;
        using var content = CreateMultipartFormDataContent();

        // act
        var message = await httpClient.PostAsync($"api/animals/{animalId}/images", content);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddAnimalImages_WithInvalidAnimalId_ReturnsNotFound()
    {
        // arrange
        const int animalId = -1;
        using var content = CreateMultipartFormDataContent();

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
