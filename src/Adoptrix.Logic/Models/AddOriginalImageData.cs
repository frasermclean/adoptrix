namespace Adoptrix.Logic.Models;

public class AddOriginalImageData
{
    public required string FileName { get; init; }
    public required string Description { get; init; }
    public required string ContentType { get; init; }
    public required Stream Stream { get; init; }

}
