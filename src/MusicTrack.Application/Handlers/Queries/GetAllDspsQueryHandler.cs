using FluentResults;
using MediatR;
using MusicTrack.Application.Mappers;
using MusicTrack.Application.Queries;
using MusicTrack.Application.Responses;
using MusicTrack.Core.Interfaces;

namespace MusicTrack.Application.Handlers.Queries;

public sealed class GetAllDspsQueryHandler : IRequestHandler<GetAllDspsQuery, Result<IEnumerable<DspResponseDto>>>
{
    private readonly IDspRepository _dspRepository;
    private readonly DspMapper _mapper;

    public GetAllDspsQueryHandler(IDspRepository dspRepository, DspMapper mapper)
    {
        _dspRepository = dspRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<DspResponseDto>>> Handle(GetAllDspsQuery request, CancellationToken cancellationToken)
    {
        var dsps = await _dspRepository.GetAllAsync(cancellationToken);
        return Result.Ok(_mapper.ToResponseListDto(dsps));
    }
}
