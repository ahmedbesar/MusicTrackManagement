using MusicTrack.Core.Entities;

namespace MusicTrack.Core.Interfaces;

public interface IArtistRepository
{
    Task<Artist?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Artist>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Artist> AddAsync(Artist artist, CancellationToken cancellationToken = default);
}
