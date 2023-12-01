using Adoptrix.Domain.Errors;
using FluentResults;

namespace Adoptrix.Domain;

public class Animal : Aggregate
{
    public const int NameMaxLength = 50;
    public const int DescriptionMaxLength = 2000;

    private readonly List<ImageInformation> images = new();

    public required string Name { get; set; }
    public string? Description { get; set; }
    public required Species Species { get; set; }
    public Breed? Breed { get; set; }
    public Sex? Sex { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public IReadOnlyList<ImageInformation> Images => images;

    public Result<ImageInformation> AddImage(string fileName, string? description, string originalFileName,
        Guid? uploadedBy = null)
    {
        if (ImageExists(fileName))
        {
            return new DuplicateImageError(fileName);
        }

        var image = new ImageInformation
        {
            FileName = fileName,
            Description = description,
            OriginalFileName = originalFileName,
            UploadedBy = uploadedBy
        };

        images.Add(image);

        return image;
    }

    public bool ImageExists(string fileName) => Images.Any(image => image.FileName == fileName);
}