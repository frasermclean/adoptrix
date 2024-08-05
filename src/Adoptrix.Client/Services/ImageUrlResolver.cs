namespace Adoptrix.Client.Services;

public class ImageUrlResolver(string baseUrl)
{
    public string GetPreviewUrl(int animalId, int? imageId) => GetUrl(animalId, imageId, "preview");
    public string GetThumbUrl(int animalId, int? imageId) => GetUrl(animalId, imageId, "thumb");
    public string GetFullSizeUrl(int animalId, int? imageId) => GetUrl(animalId, imageId, "full");

    private string GetUrl(int animalId, int? imageId, string size)
        => imageId is null ? string.Empty : $"{baseUrl}/{animalId}/{imageId}/{size}.webp";
}
