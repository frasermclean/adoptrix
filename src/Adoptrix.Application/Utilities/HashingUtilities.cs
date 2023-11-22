using System.Security.Cryptography;
using System.Text;

namespace Adoptrix.Application.Utilities;

public static class HashingUtilities
{
    /// <summary>
    /// Computes a hash from the given values.
    /// </summary>
    /// <param name="values">The string values to derive the hash from.</param>
    /// <returns>The computed hash string in Base64 format.</returns>
    public static string ComputeHash(params string[] values)
    {
        var joinedValues = string.Join("", values);
        var hash = MD5.HashData(Encoding.UTF8.GetBytes(joinedValues));

        return Convert.ToBase64String(hash);
    }
}