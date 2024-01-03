using Adoptrix.Domain;
using Bogus;

namespace Adoptrix.Api.Tests.Generators;

public static class ImageInformationGenerator
{
    private static readonly Faker<ImageInformation> ImageInformationFaker = new Faker<ImageInformation>()
        .RuleFor(imageInformation => imageInformation.Id, Guid.NewGuid)
        .RuleFor(imageInformation => imageInformation.Description, faker => faker.Lorem.Paragraph());

    public static IEnumerable<ImageInformation> Generate(int count) => ImageInformationFaker.Generate(count);
}