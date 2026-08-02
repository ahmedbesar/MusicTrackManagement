using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicTrack.Core.Interfaces;
using MusicTrack.Infrastructure.Data;
using MusicTrack.Infrastructure.Repositories;

namespace MusicTrack.Infrastructure.Extensions;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MusicTrackDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MusicTrackConnection")));

        services.AddScoped<IArtistRepository, ArtistRepository>();
        services.AddScoped<ITrackRepository, TrackRepository>();
        services.AddScoped<IDspRepository, DspRepository>();

        return services;
    }
}
