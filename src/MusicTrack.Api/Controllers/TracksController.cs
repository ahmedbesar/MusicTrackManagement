using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicTrack.Api.Extensions;
using MusicTrack.Application.Commands;
using MusicTrack.Application.Queries;
using MusicTrack.Core.Enums;

namespace MusicTrack.Api.Controllers;

public class TracksController : BaseApiController
{
    private readonly IMediator _mediator;

    public TracksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateTrackCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.ToHttpResponse();
    }

    [HttpGet]
    public async Task<ActionResult> GetAll(
        [FromQuery] Guid? artistId,
        [FromQuery] string? genre,
        [FromQuery] TrackStatus? status,
        CancellationToken cancellationToken)
    {
        var query = new GetAllTracksQuery { ArtistId = artistId, Genre = genre, Status = status };
        var result = await _mediator.Send(query, cancellationToken);
        return result.ToHttpResponse();
    }
}
