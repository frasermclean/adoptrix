namespace Adoptrix.Application.Services;

public interface IEventPublisher
{
    Task PublishAnimalDeletedEventAsync(Guid animalId, CancellationToken cancellationToken = default);
    Task PublishAnimalImageAddedEventAsync(Guid animalId, Guid imageId, CancellationToken cancellationToken = default);
}