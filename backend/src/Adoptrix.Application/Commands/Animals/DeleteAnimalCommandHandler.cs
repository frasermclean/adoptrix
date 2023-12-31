﻿using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Events;
using FastEndpoints;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Commands.Animals;

public class DeleteAnimalCommandHandler(
    ILogger<DeleteAnimalCommandHandler> logger,
    IAnimalsRepository repository,
    IEventPublisher eventPublisher)
    : ICommandHandler<DeleteAnimalCommand, Result>
{
    public async Task<Result> ExecuteAsync(DeleteAnimalCommand command, CancellationToken cancellationToken)
    {
        // find existing animal
        var result = await repository.GetAsync(command.Id, cancellationToken);
        if (result.IsFailed)
        {
            logger.LogError("Could not delete animal with id {Id}", command.Id);
            return result.ToResult();
        }

        var animal = result.Value;
        var deleteResult = await repository.DeleteAsync(animal, cancellationToken);

        // publish domain event
        var domainEvent = new AnimalDeletedEvent(animal.Id);
        await eventPublisher.PublishDomainEventAsync(domainEvent, cancellationToken);

        return deleteResult;
    }
}