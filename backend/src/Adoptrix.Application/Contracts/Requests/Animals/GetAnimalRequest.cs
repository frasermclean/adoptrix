using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Contracts.Requests.Animals;

public record GetAnimalRequest(Guid AnimalId) : IRequest<Result<Animal>>;
