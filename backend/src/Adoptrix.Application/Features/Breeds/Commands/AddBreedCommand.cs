using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Breeds.Commands;

public record AddBreedCommand(string Name, Guid SpeciesId, Guid UserId) : IRequest<Result<Breed>>;
