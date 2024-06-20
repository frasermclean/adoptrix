using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Domain.Commands.Breeds;

public record AddBreedCommand(string Name, Guid SpeciesId, Guid UserId) : IRequest<Result<Breed>>;
