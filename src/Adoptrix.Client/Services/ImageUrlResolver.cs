namespace Adoptrix.Client.Services;

public class ImageUrlResolver(string baseUrl)
{
    public string GetPreviewUrl(Guid animalId, Guid? imageId) => GetUrl(animalId, imageId, "preview");
    public string GetThumbUrl(Guid animalId, Guid? imageId) => GetUrl(animalId, imageId, "thumb");
    public string GetFullSizeUrl(Guid animalId, Guid? imageId) => GetUrl(animalId, imageId, "full");

    private string GetUrl(Guid animalId, Guid? imageId, string size)
        => imageId is null ? string.Empty : $"{baseUrl}/{animalId}/{imageId}/{size}";
}
