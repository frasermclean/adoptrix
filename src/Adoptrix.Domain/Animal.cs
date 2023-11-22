using Adoptrix.Domain.Errors;
using FluentResults;

namespace Adoptrix.Domain;

public class Animal : AggregateRoot
{
    public const int NameMaxLength = 50;
    public const int DescriptionMaxLength = 2000;

    private readonly List<ImageInformation> images = new();

    public required string Name { get; set; }
    public string? Description { get; set; }
    public required Species Species { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public IReadOnlyCollection<ImageInformation> Images => images;

    /// <summary>
    /// Updates the properties of this animal with values from the given animal.
    /// </summary>
    /// <param name="animal">The <see cref="Animal"/> to update from.</param>
    public void UpdateFrom(Animal animal)
    {
        Name = animal.Name;
        Description = animal.Description;
        Species = animal.Species;
        DateOfBirth = animal.DateOfBirth;
    }

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