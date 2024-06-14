using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Animals.Queries;

public record GetAnimalQuery(string AnimalIdOrSlug) : IRequest<Result<Animal>>;
