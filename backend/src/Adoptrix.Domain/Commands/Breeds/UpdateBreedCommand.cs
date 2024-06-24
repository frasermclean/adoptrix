using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Domain.Commands.Breeds;

public record UpdateBreedCommand(Guid BreedId, string Name, Guid SpeciesId) : IRequest<Result<Breed>>;
