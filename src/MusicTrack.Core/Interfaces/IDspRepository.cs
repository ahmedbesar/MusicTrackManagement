using MusicTrack.Core.Entities;

namespace MusicTrack.Core.Interfaces;

public interface IDspRepository
{
    Task<Dsp?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Dsp>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Dsp>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
}
