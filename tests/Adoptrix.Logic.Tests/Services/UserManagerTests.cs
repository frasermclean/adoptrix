using Adoptrix.Logic.Options;
using Adoptrix.Logic.Services;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using MicrosoftOptions = Microsoft.Extensions.Options.Options;

namespace Adoptrix.Logic.Tests.Services;

public class UserManagerTests
{
    private readonly Mock<IRequestAdapter> requestAdapterMock = new();
    private readonly UserManager userManager;

    public UserManagerTests()
    {
        var options = new UserManagerOptions
        {
            ClientId = string.Empty,
            ClientSecret = string.Empty,
            ApiObjectId = Guid.Empty
        };

        var graphServiceClient = new GraphServiceClient(requestAdapterMock.Object);
        userManager = new UserManager(graphServiceClient, MicrosoftOptions.Create(options));
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
        var users = await userManager.GetAllUsersAsync();

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
        var result = await userManager.GetUserAsync(Guid.Parse(user.Id!));

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
        var result = await userManager.GetUserAsync(Guid.NewGuid());

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
