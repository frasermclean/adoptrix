using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;

namespace Adoptrix.Application.Commands.Animals;

public class GetAnimalCommandHandler(IAnimalsRepository repository, ISqidConverter sqidConverter)
    : ICommandHandler<GetAnimalCommand, Result<Animal>>
{
    public async Task<Result<Animal>> ExecuteAsync(GetAnimalCommand command, CancellationToken cancellationToken)
    {
        var animalId = sqidConverter.ConvertToInt(command.Id);
        return await repository.GetAsync(animalId, cancellationToken);
    }
}