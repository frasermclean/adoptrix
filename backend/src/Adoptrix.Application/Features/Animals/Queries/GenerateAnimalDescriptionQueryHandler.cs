﻿using Adoptrix.Application.Features.Animals.Responses;
using Adoptrix.Application.Services;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Animals.Queries;

public class GenerateAnimalDescriptionQueryHandler(IAnimalAssistant animalAssistant)
    : IRequestHandler<GenerateAnimalDescriptionQuery, Result<AnimalDescriptionResponse>>
{
    public async Task<Result<AnimalDescriptionResponse>> Handle(GenerateAnimalDescriptionQuery query,
        CancellationToken cancellationToken)
    {
        return await animalAssistant.GenerateDescriptionAsync(query, cancellationToken);
    }
}
