using Adoptrix.Api.Services;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;

namespace Adoptrix.Api.Tests.Services;

public class UsersServiceTests
{
    private readonly Mock<IRequestAdapter> requestAdapterMock = new();
    private readonly UsersService usersService;

    public UsersServiceTests()
    {
        var graphServiceClient = new GraphServiceClient(requestAdapterMock.Object);
        usersService = new UsersService(graphServiceClient);
    }

    [Fact]
    public async Task GetAllUsersAsync_WithValidResponse_ShouldReturnUsers()
    {
        // arrange
        var response = new UserCollectionResponse
        {
            Value = CreateUsers(3)
        };

        requestAdapterMock.Setup(adapter => adapter.SendAsync(
                It.IsAny<RequestInformation>(),
                It.IsAny<ParsableFactory<UserCollectionResponse>>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // act
        var users = await usersService.GetAllUsersAsync();

        // assert
        users.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetUserAsync_WithValidUserId_ShouldReturnUser()
    {
        // arrange
        var user = CreateUser();
        requestAdapterMock.Setup(adapter => adapter.SendAsync(
                It.IsAny<RequestInformation>(),
                It.IsAny<ParsableFactory<User>>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // act
        var result = await usersService.GetUserAsync(Guid.Parse(user.Id!));

        // assert
        result.Should().BeSuccess();
    }

    [Fact]
    public async Task GetUserAsync_WithInvalidUserId_ShouldReturnError()
    {
        // arrange
        requestAdapterMock.Setup(adapter => adapter.SendAsync(
                It.IsAny<RequestInformation>(),
                It.IsAny<ParsableFactory<User>>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ODataError());

        // act
        var result = await usersService.GetUserAsync(Guid.NewGuid());

        // assert
        result.Should().BeFailure();
    }

    private static List<User> CreateUsers(int count) => Enumerable.Range(0, count).Select(_ => CreateUser()).ToList();

    private static User CreateUser() => new()
    {
        Id = Guid.NewGuid().ToString(),
        GivenName = "John",
        Surname = "Doe",
    };
}
