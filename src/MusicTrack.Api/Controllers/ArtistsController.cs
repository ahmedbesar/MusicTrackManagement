using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicTrack.Api.Extensions;
using MusicTrack.Application.Commands;
using MusicTrack.Application.Queries;

namespace MusicTrack.Api.Controllers;

public class ArtistsController : BaseApiController
{
    private readonly IMediator _mediator;

    public ArtistsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateArtistCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.ToHttpResponse();
    }

    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllArtistsQuery(), cancellationToken);
        return result.ToHttpResponse();
    }
}
