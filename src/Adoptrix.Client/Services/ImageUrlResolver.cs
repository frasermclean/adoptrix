namespace Adoptrix.Client.Services;

public class ImageUrlResolver(string baseUrl)
{
    public string GetPreviewUrl(string animalSlug, Guid? imageId) => GetUrl(animalSlug, imageId, "preview");
    public string GetThumbUrl(string animalSlug, Guid? imageId) => GetUrl(animalSlug, imageId, "thumb");
    public string GetFullSizeUrl(string animalSlug, Guid? imageId) => GetUrl(animalSlug, imageId, "full");

    private string GetUrl(string animalSlug, Guid? imageId, string size)
        => imageId is null ? string.Empty : $"{baseUrl}/{animalSlug}/{imageId}/{size}.webp";
}
