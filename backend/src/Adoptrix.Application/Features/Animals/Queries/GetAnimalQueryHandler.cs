using Adoptrix.Application.Mapping;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Errors;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Animals;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Animals.Queries;

public class GetAnimalQueryHandler(IAnimalsRepository animalsRepository) : IRequestHandler<GetAnimalQuery, Result<AnimalResponse>>
{
    public async Task<Result<AnimalResponse>> Handle(GetAnimalQuery query, CancellationToken cancellationToken)
    {
        var animal = await animalsRepository.GetByIdAsync(query.AnimalId, cancellationToken);

        if (animal is null)
        {
            return new AnimalNotFoundError(query.AnimalId);
        }

        var response =  animal.ToResponse();



        return response;
    }
}
