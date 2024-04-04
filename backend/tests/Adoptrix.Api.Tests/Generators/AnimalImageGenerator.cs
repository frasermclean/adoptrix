using Adoptrix.Domain.Models;
using Bogus;

namespace Adoptrix.Api.Tests.Generators;

public static class AnimalImageGenerator
{
    private static readonly Faker<AnimalImage> ImageInformationFaker = new Faker<AnimalImage>()
        .RuleFor(imageInformation => imageInformation.Id, faker => faker.Random.Guid())
        .RuleFor(imageInformation => imageInformation.Description, faker => faker.Lorem.Paragraph())
        .RuleFor(imageInformation => imageInformation.OriginalFileName, faker => faker.Random.String())
        .RuleFor(imageInformation => imageInformation.OriginalContentType, faker => faker.Random.String())
        .RuleFor(imageInformation => imageInformation.IsProcessed, faker => faker.Random.Bool())
        .RuleFor(imageInformation => imageInformation.UploadedBy, faker => faker.Random.Guid())
        .RuleFor(imageInformation => imageInformation.UploadedAt, faker => faker.Date.Past());

    public static AnimalImage Generate() => ImageInformationFaker.Generate();

    public static IEnumerable<AnimalImage> Generate(int count) => ImageInformationFaker.Generate(count);
}
