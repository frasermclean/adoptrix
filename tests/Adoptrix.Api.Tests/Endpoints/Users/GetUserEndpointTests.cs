using System.Net;
using Adoptrix.Api.Endpoints.Users;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Contracts.Responses;
using FluentResults;

namespace Adoptrix.Api.Tests.Endpoints.Users;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class GetUserEndpointTests(MockServicesFixture fixture) : TestBase<MockServicesFixture>
{
    private readonly HttpClient httpClient = fixture.CreateClient();

    [Fact]
    public async Task GetUser_WithValidId_ShouldReturnOk()
    {
        // arrange
        var userId = Guid.NewGuid();
        var request = new GetUserRequest(userId);
        fixture.UsersServiceMock.Setup(service => service.GetUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserResponse { Id = userId });

        // act
        var (message, response) = await httpClient.GETAsync<GetUserEndpoint, GetUserRequest, UserResponse>(request);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Id.Should().Be(userId);
    }

    [Fact]
    public async Task GetUser_WithInvalidId_ShouldReturnNotFound()
    {
        // arrange
        var userId = Guid.NewGuid();
        var request = new GetUserRequest(userId);
        fixture.UsersServiceMock.Setup(service => service.GetUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<UserResponse>("User not found"));

        // act
        var message = await httpClient.GETAsync<GetUserEndpoint, GetUserRequest>(request);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
