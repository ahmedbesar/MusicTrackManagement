using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MusicTrack.Infrastructure.Data;

/// <summary>
/// Enables `dotnet ef` design-time tooling to create the DbContext without
/// depending on the Api project's runtime DI wiring. Reads the connection
/// string from the startup project's appsettings.json so there is a single
/// source of truth (dotnet ef sets the current directory to the
/// --startup-project directory when this factory is invoked).
/// </summary>
public class MusicTrackDbContextFactory : IDesignTimeDbContextFactory<MusicTrackDbContext>
{
    private const string ConnectionStringName = "MusicTrackConnection";

    public MusicTrackDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString(ConnectionStringName)
            ?? throw new InvalidOperationException(
                $"Connection string '{ConnectionStringName}' not found. Run this command from the repository root with --startup-project src/MusicTrack.Api.");

        var optionsBuilder = new DbContextOptionsBuilder<MusicTrackDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new MusicTrackDbContext(optionsBuilder.Options);
    }
}
