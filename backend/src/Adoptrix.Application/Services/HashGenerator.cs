using System.Security.Cryptography;
using System.Text;

namespace Adoptrix.Application.Services;

public interface IHashGenerator
{
    /// <summary>
    /// Computes a hash from the given values.
    /// </summary>
    /// <param name="values">The string values to derive the hash from.</param>
    /// <returns>The computed hash string in lowercase hexadecimal format.</returns>
    string ComputeHash(params string[] values);
}

public class HashGenerator : IHashGenerator
{
    public string ComputeHash(params string[] values)
    {
        var joinedValues = string.Join("", values);
        var hash = MD5.HashData(Encoding.UTF8.GetBytes(joinedValues));

        return Convert.ToHexString(hash).ToLower();
    }
}