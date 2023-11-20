using Adoptrix.Domain;

namespace Adoptrix.Api.Contracts;

public class UpdateAnimalRequest
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required Species Species { get; init; }
    public required DateOnly DateOfBirth { get; init; }
}