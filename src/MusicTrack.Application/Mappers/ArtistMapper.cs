using MusicTrack.Application.Responses;
using MusicTrack.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace MusicTrack.Application.Mappers;

[Mapper]
public partial class ArtistMapper
{
    [MapperIgnoreSource(nameof(Artist.Tracks))]
    public partial ArtistResponseDto ToResponseDto(Artist artist);

    public partial IEnumerable<ArtistResponseDto> ToResponseListDto(IEnumerable<Artist> artists);
}
