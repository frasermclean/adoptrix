namespace Adoptrix.Client.Services;

public class ImageUrlResolver(string baseUrl)
{
    public string GetPreviewUrl(int animalId, Guid? imageId) => GetUrl(animalId, imageId, "preview");
    public string GetThumbUrl(int animalId, Guid? imageId) => GetUrl(animalId, imageId, "thumb");
    public string GetFullSizeUrl(int animalId, Guid? imageId) => GetUrl(animalId, imageId, "full");

    private string GetUrl(int animalId, Guid? imageId, string size)
        => imageId is null ? string.Empty : $"{baseUrl}/{animalId}/{imageId}/{size}";
}
