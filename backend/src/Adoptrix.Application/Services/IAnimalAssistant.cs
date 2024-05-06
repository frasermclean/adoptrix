using Adoptrix.Application.Features.Animals.Queries;
using Adoptrix.Application.Features.Animals.Responses;

namespace Adoptrix.Application.Services;

/// <summary>
/// Assistant service which uses artificial intelligence to generate data for an animal.
/// </summary>
public interface IAnimalAssistant
{
    Task<AnimalDescriptionResponse> GenerateDescriptionAsync(GenerateAnimalDescriptionQuery query, CancellationToken cancellationToken = default);
}
