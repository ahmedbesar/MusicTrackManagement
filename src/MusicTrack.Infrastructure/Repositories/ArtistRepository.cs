using Microsoft.EntityFrameworkCore;
using MusicTrack.Core.Entities;
using MusicTrack.Core.Interfaces;
using MusicTrack.Infrastructure.Data;

namespace MusicTrack.Infrastructure.Repositories;

public class ArtistRepository : IArtistRepository
{
    private readonly MusicTrackDbContext _context;

    public ArtistRepository(MusicTrackDbContext context)
    {
        _context = context;
    }

    public async Task<Artist?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Artists
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Artist>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Artists
            .AsNoTracking()
            .OrderBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Artists.AsNoTracking().AnyAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Artist> AddAsync(Artist artist, CancellationToken cancellationToken = default)
    {
        await _context.Artists.AddAsync(artist, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return artist;
    }
}
