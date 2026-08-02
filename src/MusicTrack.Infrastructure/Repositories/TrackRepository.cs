using Microsoft.EntityFrameworkCore;
using MusicTrack.Core.Entities;
using MusicTrack.Core.Enums;
using MusicTrack.Core.Interfaces;
using MusicTrack.Infrastructure.Data;

namespace MusicTrack.Infrastructure.Repositories;

public class TrackRepository : ITrackRepository
{
    private readonly MusicTrackDbContext _context;

    public TrackRepository(MusicTrackDbContext context)
    {
        _context = context;
    }

    public async Task<Track?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tracks
            .Include(t => t.Artist)
            .Include(t => t.Distributions)
                .ThenInclude(d => d.Dsp)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Track>> GetAllAsync(
        Guid? artistId,
        string? genre,
        TrackStatus? status,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Tracks
            .Include(t => t.Artist)
            .AsNoTracking()
            .AsQueryable();

        if (artistId is not null)
            query = query.Where(t => t.ArtistId == artistId);

        if (!string.IsNullOrWhiteSpace(genre))
            query = query.Where(t => t.Genre == genre);

        if (status is not null)
            query = query.Where(t => t.Status == status);

        return await query
            .OrderByDescending(t => t.ReleaseDate)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> IsrcExistsAsync(string isrc, CancellationToken cancellationToken = default)
    {
        return _context.Tracks.AsNoTracking().AnyAsync(t => t.Isrc == isrc, cancellationToken);
    }

    public async Task<Track> AddAsync(Track track, CancellationToken cancellationToken = default)
    {
        await _context.Tracks.AddAsync(track, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return track;
    }

    public Task<bool> DistributionExistsAsync(Guid trackId, Guid dspId, CancellationToken cancellationToken = default)
    {
        return _context.TrackDistributions
            .AsNoTracking()
            .AnyAsync(d => d.TrackId == trackId && d.DspId == dspId, cancellationToken);
    }

    public async Task AddDistributionAsync(TrackDistribution distribution, CancellationToken cancellationToken = default)
    {
        await _context.TrackDistributions.AddAsync(distribution, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
