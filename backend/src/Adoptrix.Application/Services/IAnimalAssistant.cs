using Adoptrix.Application.Features.Generators.Queries;
using Adoptrix.Application.Features.Generators.Responses;

namespace Adoptrix.Application.Services;

/// <summary>
/// Assistant service which uses artificial intelligence to generate data for an animal.
/// </summary>
public interface IAnimalAssistant
{
    Task<AnimalNameResponse> GenerateNameAsync(GenerateAnimalNameQuery query,
        CancellationToken cancellationToken = default);

    Task<AnimalDescriptionResponse> GenerateDescriptionAsync(GenerateAnimalDescriptionQuery query,
        CancellationToken cancellationToken = default);
}
