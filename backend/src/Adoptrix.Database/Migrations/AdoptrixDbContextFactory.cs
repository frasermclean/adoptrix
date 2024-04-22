using Adoptrix.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Adoptrix.Database.Migrations;

public class AdoptrixDbContextFactory : IDesignTimeDbContextFactory<AdoptrixDbContext>
{
    private const string ConnectionString =
        "Server=localhost;Database=AdoptrixDb;User ID=sa;Password=sup3rSECRET!;TrustServerCertificate=True";

    public AdoptrixDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AdoptrixDbContext>();

        optionsBuilder.UseSqlServer(ConnectionString);

        return new AdoptrixDbContext(optionsBuilder.Options);
    }
}
