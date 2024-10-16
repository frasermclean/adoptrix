using System.Net;
using System.Net.Http.Headers;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Initializer;

namespace Adoptrix.Api.Tests.Endpoints.Animals.Images;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class AddAnimalImagesEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    [Fact]
    public async Task AddAnimalImages_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        var animalId = SeedData.Alberto.Id;
        using var content = CreateMultipartFormDataContent();

        // act
        var message = await fixture.AdminClient.PostAsync($"api/animals/{animalId}/images", content);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddAnimalImages_WithInvalidAnimalId_ReturnsNotFound()
    {
        // arrange
        var animalId = Guid.NewGuid();
        using var content = CreateMultipartFormDataContent();

        // act
        var message = await fixture.AdminClient.PostAsync($"api/animals/{animalId}/images", content);

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
