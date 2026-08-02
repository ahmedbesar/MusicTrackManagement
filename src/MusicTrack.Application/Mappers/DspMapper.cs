using MusicTrack.Application.Responses;
using MusicTrack.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace MusicTrack.Application.Mappers;

[Mapper]
public partial class DspMapper
{
    [MapperIgnoreSource(nameof(Dsp.Distributions))]
    public partial DspResponseDto ToResponseDto(Dsp dsp);

    public partial IEnumerable<DspResponseDto> ToResponseListDto(IEnumerable<Dsp> dsps);
}
