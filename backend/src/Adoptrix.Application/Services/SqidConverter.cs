using Microsoft.Extensions.Options;
using Sqids;

namespace Adoptrix.Application.Services;

public interface ISqidConverter
{
    int ConvertToInt(string sqid, bool shouldThrowOnError = true);
    string ConvertToSqid(int id);
    bool IsValid(string sqid);
}

public class SqidConverter(IOptions<SqidsOptions> options)
    : ISqidConverter
{
    private readonly SqidsEncoder<int> encoder = new(options.Value);

    private const int InvalidValue = -1;

    public int ConvertToInt(string sqid, bool shouldThrowOnError = true)
    {
        var numbers = encoder.Decode(sqid);

        if (numbers.Count == 1)
        {
            return numbers[0];
        }

        if (shouldThrowOnError)
        {
            throw new ArgumentException("Invalid value to decode", nameof(sqid));
        }

        return InvalidValue;
    }

    public string ConvertToSqid(int id)
    {
        return encoder.Encode(id);
    }

    public bool IsValid(string sqid)
    {
        var result = ConvertToInt(sqid, false);
        if (result <= InvalidValue)
        {
            return false;
        }

        return ConvertToSqid(result) == sqid;
    }
}