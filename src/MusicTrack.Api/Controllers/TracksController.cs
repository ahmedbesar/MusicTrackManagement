using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicTrack.Api.Extensions;
using MusicTrack.Application.Commands;

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
}
