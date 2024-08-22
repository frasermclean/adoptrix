namespace Adoptrix.Client.Services;

public class ImageUrlResolver(string baseUrl)
{
    public string GetPreviewUrl(string animalSlug, int? imageId) => GetUrl(animalSlug, imageId, "preview");
    public string GetThumbUrl(string animalSlug, int? imageId) => GetUrl(animalSlug, imageId, "thumb");
    public string GetFullSizeUrl(string animalSlug, int? imageId) => GetUrl(animalSlug, imageId, "full");

    private string GetUrl(string animalSlug, int? imageId, string size)
        => imageId is null ? string.Empty : $"{baseUrl}/{animalSlug}/{imageId}/{size}.webp";
}
