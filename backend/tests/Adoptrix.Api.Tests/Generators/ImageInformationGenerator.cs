using Adoptrix.Domain;
using Bogus;

namespace Adoptrix.Api.Tests.Generators;

public static class ImageInformationGenerator
{
    private static readonly Faker<ImageInformation> ImageInformationFaker = new Faker<ImageInformation>()
        .RuleFor(imageInformation => imageInformation.Id, faker => faker.Random.Guid())
        .RuleFor(imageInformation => imageInformation.Description, faker => faker.Lorem.Paragraph())
        .RuleFor(imageInformation => imageInformation.OriginalFileName, faker => faker.Random.String())
        .RuleFor(imageInformation => imageInformation.OriginalContentType, faker => faker.Random.String())
        .RuleFor(imageInformation => imageInformation.IsProcessed, faker => faker.Random.Bool())
        .RuleFor(imageInformation => imageInformation.UploadedBy, faker => faker.Random.Guid())
        .RuleFor(imageInformation => imageInformation.UploadedAt, faker => faker.Date.Past());

    public static ImageInformation Generate() => ImageInformationFaker.Generate();

    public static IEnumerable<ImageInformation> Generate(int count) => ImageInformationFaker.Generate(count);
}