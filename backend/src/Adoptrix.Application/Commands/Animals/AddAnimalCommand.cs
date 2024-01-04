using System.Text.Json.Serialization;
using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;
using Microsoft.Identity.Web;

namespace Adoptrix.Application.Commands.Animals;

public class AddAnimalCommand : ICommand<Result<Animal>>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    [JsonPropertyName("species")] public required string SpeciesName { get; init; }
    [JsonPropertyName("breed")]public string? BreedName { get; init; }
    public Sex? Sex { get; init; }
    public required DateOnly DateOfBirth { get; init; }

    [FromClaim(ClaimConstants.NameIdentifierId,
        false)] // TODO: Find solution to make this required as it currently breaks tests
    public Guid UserId { get; init; }
}