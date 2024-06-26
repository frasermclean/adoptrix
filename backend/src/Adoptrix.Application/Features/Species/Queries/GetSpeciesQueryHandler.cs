﻿using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
using FluentResults;
using MediatR;
using SpeciesModel = Adoptrix.Domain.Models.Species;

namespace Adoptrix.Application.Features.Species.Queries;

public class GetSpeciesQueryHandler(ISpeciesRepository speciesRepository)
    : IRequestHandler<GetSpeciesQuery, Result<SpeciesModel>>
{
    public async Task<Result<SpeciesModel>> Handle(GetSpeciesQuery query, CancellationToken cancellationToken)
    {
        var species = await speciesRepository.GetByIdAsync(query.SpeciesId, cancellationToken);

        return species is null
            ? new SpeciesNotFoundError(query.SpeciesId)
            : species;
    }
}
