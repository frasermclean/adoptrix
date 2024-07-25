using System.Security.Cryptography;
using System.Text;

namespace Adoptrix.Client.Services;

public class GravatarUrlGenerator
{
    public string GetGravatarUrl(string emailAddress, int size = 40, string defaultImage = "wavatar")
    {
        var hash = HashString(emailAddress);
        return $"https://gravatar.com/avatar/{hash}?s={size}&d={defaultImage}";
    }

    private static string HashString(string s)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(s.Trim()));
        return Convert.ToHexString(bytes).ToLower();
    }
}
