using Adoptrix.Domain.Extensions;

namespace Adoptrix.Domain;

public class Animal : Aggregate
{
    public const int NameMaxLength = 50;
    public const int DescriptionMaxLength = 2000;

    public required string Name { get; set; }
    public string? Description { get; set; }
    public required Breed Breed { get; set; }
    public required Sex Sex { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public string Age => DateOfBirth.ToAgeString();
    public List<AnimalImage> Images { get; init; } = [];

    public AnimalImage AddImage(string originalFileName, string originalContentType,
        string? description = null, Guid? uploadedBy = null)
    {
        if (Images.Any(image => image.OriginalFileName == originalFileName))
        {
            throw new ArgumentException("Image with the same name already exists", nameof(originalFileName));
        }

        var image = new AnimalImage
        {
            Description = description,
            OriginalFileName = originalFileName,
            OriginalContentType = originalContentType,
            UploadedBy = uploadedBy
        };

        Images.Add(image);
        return image;
    }
}
