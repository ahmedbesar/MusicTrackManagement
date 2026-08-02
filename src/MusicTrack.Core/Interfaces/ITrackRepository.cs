using MusicTrack.Core.Entities;
using MusicTrack.Core.Enums;

namespace MusicTrack.Core.Interfaces;

public interface ITrackRepository
{
    Task<Track?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Track>> GetAllAsync(
        Guid? artistId,
        string? genre,
        TrackStatus? status,
        CancellationToken cancellationToken = default);

    Task<bool> IsrcExistsAsync(string isrc, CancellationToken cancellationToken = default);

    Task<Track> AddAsync(Track track, CancellationToken cancellationToken = default);

    Task<bool> DistributionExistsAsync(Guid trackId, Guid dspId, CancellationToken cancellationToken = default);

    Task AddDistributionAsync(TrackDistribution distribution, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
