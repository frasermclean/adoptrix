namespace Adoptrix.Client.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Decodes a Base64URL encoded string. Returns a Base64 encoded string.
    /// </summary>
    /// <param name="value">The Base64URL string to decode</param>
    /// <returns>A Base64 encoded string</returns>
    public static string DecodeBase64Url(this string value) => value
        .Replace('-', '+')
        .Replace('_', '/')
        .PadRight(value.Length + (4 - value.Length % 4) % 4, '=');
}
