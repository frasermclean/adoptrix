using Adoptrix.Domain.Errors;
using FluentResults;

namespace Adoptrix.Domain;

public class Animal : Aggregate
{
    public const int NameMaxLength = 50;
    public const int DescriptionMaxLength = 2000;

    public required string Name { get; set; }
    public string? Description { get; set; }
    public required Species Species { get; set; }
    public Breed? Breed { get; set; }
    public Sex? Sex { get; set; }
    public required DateOnly DateOfBirth { get; set; }

    public ICollection<ImageInformation> Images { get; init; } = new List<ImageInformation>();

    public Result<ImageInformation> AddImage(string originalFileName, string originalContentType,
        string? description = null, Guid? uploadedBy = null)
    {
        if (Images.Any(image => image.OriginalFileName == originalFileName))
        {
            return new DuplicateImageError(originalFileName);
        }

        var image = new ImageInformation
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