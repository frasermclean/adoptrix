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
        return encoder.Decode(sqid)[0];
    }

    public string ConvertToSqid(int id)
    {
        return encoder.Encode(id);
    }
}