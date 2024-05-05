using Adoptrix.Application.Features.Animals.Responses;
using Adoptrix.Domain.Models;
using MediatR;

namespace Adoptrix.Application.Features.Animals.Queries;

public record GenerateAnimalDescriptionQuery(
    string AnimalName,
    string BreedName,
    string SpeciesName,
    Sex Sex,
    DateOnly DateOfBirth) : IRequest<GenerateAnimalDescriptionResponse>;
