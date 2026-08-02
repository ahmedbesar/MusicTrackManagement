using MusicTrack.Application.Responses;
using MusicTrack.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace MusicTrack.Application.Mappers;

[Mapper]
public partial class TrackMapper
{
    [MapperIgnoreSource(nameof(Track.Distributions))]
    public partial TrackResponseDto ToResponseDto(Track track);

    public partial IEnumerable<TrackResponseDto> ToResponseListDto(IEnumerable<Track> tracks);
}
