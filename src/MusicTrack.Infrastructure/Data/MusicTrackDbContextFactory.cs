using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MusicTrack.Infrastructure.Data;

/// <summary>
/// Enables `dotnet ef` design-time tooling to create the DbContext without
/// depending on the Api project's runtime DI wiring.
/// </summary>
public class MusicTrackDbContextFactory : IDesignTimeDbContextFactory<MusicTrackDbContext>
{
    private const string DesignTimeConnectionString =
        "Server=(localdb)\\MSSQLLocalDB;Database=MusicTrackDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";

    public MusicTrackDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MusicTrackDbContext>();
        optionsBuilder.UseSqlServer(DesignTimeConnectionString);

        return new MusicTrackDbContext(optionsBuilder.Options);
    }
}
