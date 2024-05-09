using Adoptrix.Application.Features.Generators.Responses;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Generators.Queries;

public record GenerateAnimalDescriptionQuery(
    string AnimalName,
    string BreedName,
    string SpeciesName,
    Sex Sex,
    DateOnly DateOfBirth) : IRequest<Result<AnimalDescriptionResponse>>;
