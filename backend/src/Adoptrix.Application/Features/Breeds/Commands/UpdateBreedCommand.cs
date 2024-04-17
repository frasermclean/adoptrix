using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Breeds.Commands;

public record UpdateBreedCommand(Guid BreedId, string BreedName, Guid SpeciesId) : IRequest<Result<Breed>>;
