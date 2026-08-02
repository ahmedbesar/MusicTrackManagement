using Microsoft.EntityFrameworkCore;
using MusicTrack.Core.Entities;
using MusicTrack.Core.Interfaces;
using MusicTrack.Infrastructure.Data;

namespace MusicTrack.Infrastructure.Repositories;

public class DspRepository : IDspRepository
{
    private readonly MusicTrackDbContext _context;

    public DspRepository(MusicTrackDbContext context)
    {
        _context = context;
    }

    public async Task<Dsp?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Dsps
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Dsp>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Dsps
            .AsNoTracking()
            .OrderBy(d => d.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Dsp>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await _context.Dsps
            .AsNoTracking()
            .Where(d => ids.Contains(d.Id))
            .ToListAsync(cancellationToken);
    }
}
