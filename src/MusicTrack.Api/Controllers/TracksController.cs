using MediatR;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTrackByIdQuery(id), cancellationToken);
        return result.ToHttpResponse();
    }

    [Authorize]
    [HttpPost("{id:guid}/distribute")]
    public async Task<ActionResult> Distribute(Guid id, [FromBody] DistributeTrackCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command with { TrackId = id }, cancellationToken);
        return result.ToHttpResponse();
    }
}
