using Adoptrix.Application.Features.Generators.Responses;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Generators.Queries;

public record GenerateAnimalNameQuery(string? SpeciesName = null, string? BreedName = null, Sex? Sex = null)
    : IRequest<Result<AnimalNameResponse>>;
