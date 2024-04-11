using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Contracts.Requests.Animals;

public record AddAnimalImagesRequest(Guid AnimalId, Guid UserId, IEnumerable<AnimalImageFileData> FileData)
    : IRequest<Result<Animal>>;

public record AnimalImageFileData(string FileName, string Description, string ContentType, long Length, Stream Stream);
