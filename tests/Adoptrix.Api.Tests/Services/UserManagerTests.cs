using Adoptrix.Api.Errors;
using Adoptrix.Api.Options;
using Adoptrix.Api.Security;
using Adoptrix.Api.Services;
using Adoptrix.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;

namespace Adoptrix.Api.Tests.Services;

public class UserManagerTests
{
    private static readonly Guid ApiObjectId = Guid.NewGuid();
    private readonly Mock<IRequestAdapter> requestAdapterMock;
    private readonly UserManager userManager;

    public UserManagerTests()
    {
        // configure graph service client to use mocked request adapter
        var serializationWriterFactoryMock = new Mock<ISerializationWriterFactory>();
        serializationWriterFactoryMock.Setup(factory => factory.GetSerializationWriter(It.IsAny<string>()))
            .Returns(new JsonSerializationWriter());
        requestAdapterMock = new Mock<IRequestAdapter>();
        requestAdapterMock.SetupGet(adapter => adapter.SerializationWriterFactory)
            .Returns(serializationWriterFactoryMock.Object);
        var graphServiceClient = new GraphServiceClient(requestAdapterMock.Object);

        // configure mocked options
        var optionsMock = new Mock<IOptions<UserManagerOptions>>();
        optionsMock.Setup(mock => mock.Value).Returns(new UserManagerOptions
        {
            ClientId = "abc",
            ClientSecret = "xyz",
            ApiObjectId = ApiObjectId
        });

        userManager = new UserManager(graphServiceClient, optionsMock.Object, Mock.Of<ILogger<UserManager>>());
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
                It.Is<RequestInformation>(info => info.HttpMethod == Method.GET),
                User.CreateFromDiscriminatorValue,
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        requestAdapterMock.Setup(adapter => adapter.SendAsync(
                It.Is<RequestInformation>(info => info.HttpMethod == Method.GET),
                AppRoleAssignmentCollectionResponse.CreateFromDiscriminatorValue,
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AppRoleAssignmentCollectionResponse
            {
                Value =
                [
                    new AppRoleAssignment
                    {
                        Id = Guid.NewGuid().ToString(),
                        PrincipalId = Guid.Parse(user.Id!),
                        ResourceId = ApiObjectId,
                        AppRoleId = AppRoleIdMapping.AdministratorRoleId,
                    }
                ]
            });

        // act
        var result = await userManager.GetUserAsync(Guid.Parse(user.Id!));

        // assert
        result.Should().BeSuccess();
        result.Value.FirstName.Should().Be(user.GivenName);
        result.Value.LastName.Should().Be(user.Surname);
        result.Value.DisplayName.Should().Be(user.DisplayName);
        result.Value.Roles.Should().ContainSingle(role => role == UserRole.Administrator);
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
            .ThrowsAsync(new ODataError { ResponseStatusCode = 404 });

        // act
        var result = await userManager.GetUserAsync(Guid.NewGuid());

        // assert
        result.Should().BeFailure();
    }

    [Fact]
    public async Task AddUserRoleAssignmentAsync_WithValidRole_ShouldReturnSuccess()
    {
        // arrange
        var userId = Guid.NewGuid();
        var user = CreateUser(userId);
        const UserRole userRole = UserRole.Administrator;
        requestAdapterMock.Setup(adapter => adapter.SendAsync(
                It.Is<RequestInformation>(info => info.HttpMethod == Method.POST),
                AppRoleAssignment.CreateFromDiscriminatorValue,
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AppRoleAssignment
            {
                Id = Guid.NewGuid().ToString(),
                PrincipalId = userId,
                ResourceId = ApiObjectId,
                AppRoleId = AppRoleIdMapping.AdministratorRoleId,
            });

        requestAdapterMock.Setup(adapter => adapter.SendAsync(
                It.Is<RequestInformation>(info => info.HttpMethod == Method.GET),
                User.CreateFromDiscriminatorValue,
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // act
        var result = await userManager.AddUserRoleAssignmentAsync(userId, userRole);

        // assert
        result.Should().BeSuccess();
    }

    [Fact]
    public async Task AddUserRoleAssignmentAsync_WhenRoleIsAlreadyAssigned_ShouldReturnError()
    {
        // arrange
        var userId = Guid.NewGuid();
        const UserRole userRole = UserRole.User;
        requestAdapterMock.Setup(adapter => adapter.SendAsync(
                It.Is<RequestInformation>(info => info.HttpMethod == Method.POST),
                AppRoleAssignment.CreateFromDiscriminatorValue,
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ODataError { ResponseStatusCode = 400 });

        // act
        var result = await userManager.AddUserRoleAssignmentAsync(userId, userRole);

        // assert
        result.Should().BeFailure().Which.Should()
            .HaveReason<UserRoleAlreadyAssignedError>($"User role {userRole} is already assigned to this user");
    }

    [Fact]
    public async Task AddUserRoleAssignmentAsync_WhenUserDoesNotExist_ShouldReturnError()
    {
        // arrange
        var userId = Guid.NewGuid();
        requestAdapterMock.Setup(adapter => adapter.SendAsync(
                It.Is<RequestInformation>(info => info.HttpMethod == Method.POST),
                AppRoleAssignment.CreateFromDiscriminatorValue,
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ODataError { ResponseStatusCode = 404 });

        // act
        var result = await userManager.AddUserRoleAssignmentAsync(userId, UserRole.User);

        // assert
        result.Should().BeFailure().Which.Should()
            .HaveReason<UserNotFoundError>($"User with ID {userId} not found.");
    }

    [Fact]
    public async Task RemoveUserRoleAssignmentAsync_WithValidRole_ShouldReturnSuccess()
    {
        // arrange
        var userId = Guid.NewGuid();
        var user = CreateUser(userId);

        requestAdapterMock.Setup(adapter => adapter.SendAsync(
                It.Is<RequestInformation>(info => info.HttpMethod == Method.GET),
                AppRoleAssignmentCollectionResponse.CreateFromDiscriminatorValue,
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AppRoleAssignmentCollectionResponse
            {
                Value =
                [
                    new AppRoleAssignment
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        PrincipalId = userId,
                        ResourceId = ApiObjectId,
                        AppRoleId = AppRoleIdMapping.AdministratorRoleId,
                    }
                ]
            });

        requestAdapterMock.Setup(adapter => adapter.SendAsync(
                It.Is<RequestInformation>(info => info.HttpMethod == Method.GET),
                User.CreateFromDiscriminatorValue,
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // act
        var result = await userManager.RemoveUserRoleAssignmentAsync(userId, UserRole.Administrator);

        // assert
        result.Should().BeSuccess();
    }

    [Fact]
    public async Task RemoveUserRoleAssignmentAsync_WithNoExistentRole_ShouldReturnError()
    {
        // arrange
        var userId = Guid.NewGuid();
        requestAdapterMock.Setup(adapter => adapter.SendAsync(
                It.Is<RequestInformation>(info => info.HttpMethod == Method.GET),
                AppRoleAssignmentCollectionResponse.CreateFromDiscriminatorValue,
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AppRoleAssignmentCollectionResponse
            {
                Value = []
            });

        // act
        var result = await userManager.RemoveUserRoleAssignmentAsync(userId, UserRole.Administrator);

        // assert
        result.Should().BeFailure().Which.Should()
            .HaveReason<UserRoleNotAssignedError>($"User does not have the role {UserRole.Administrator} assigned.");
    }

    private static List<User> CreateUsers(int count) => Enumerable.Range(0, count)
        .Select(_ => CreateUser())
        .ToList();

    private static User CreateUser(Guid? userId = null) => new()
    {
        Id = userId?.ToString() ?? Guid.NewGuid().ToString(),
        GivenName = "John",
        Surname = "Doe",
        DisplayName = "John Doe",
    };
}
