using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Animals.Commands;

public record AddAnimalImagesCommand(Guid AnimalId, Guid UserId, IEnumerable<AnimalImageFileData> FileData)
    : IRequest<Result<Animal>>;

public record AnimalImageFileData(string FileName, string Description, string ContentType, long Length, Stream Stream);
