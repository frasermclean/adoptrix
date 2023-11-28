using Microsoft.Extensions.Options;
using Sqids;

namespace Adoptrix.Application.Services;

public interface ISqidConverter
{
    int CovertToInt(string sqid);
    string ConvertToSqid(int id);
}

public class SqidConverter(IOptions<SqidsOptions> options)
    : ISqidConverter
{
    private readonly SqidsEncoder<int> encoder = new(options.Value);

    public int CovertToInt(string sqid)
    {
        var numbers = encoder.Decode(sqid);
        return numbers.Count == 1
            ? numbers[0]
            : throw new ArgumentException("Invalid value to decode", nameof(sqid));
    }

    public string ConvertToSqid(int id)
    {
        return encoder.Encode(id);
    }
}