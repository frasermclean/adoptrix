using System.Text;

namespace Adoptrix.Infrastructure.Storage.Tests.Fixtures;

public class ConnectionStringBuilder
{
    public int BlobPort { get; init; } = 10000;
    public int QueuePort { get; init; } = 10001;
    public int TablePort { get; init; } = 10002;

    public string ConnectionString => BuildConnectionString();

    private const string AccountName = "devstoreaccount1";

    private const string AccountKey =
        "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";

    private string BuildConnectionString() => new StringBuilder()
        .Append("DefaultEndpointsProtocol=http;")
        .Append($"AccountName={AccountName};")
        .Append($"AccountKey={AccountKey};")
        .Append($"BlobEndpoint=http://127.0.0.1:{BlobPort}/{AccountName};")
        .Append($"QueueEndpoint=http://127.0.0.1:{QueuePort}/{AccountName};")
        .Append($"TableEndpoint=http://127.0.0.1:{TablePort}/{AccountName};")
        .ToString();
}