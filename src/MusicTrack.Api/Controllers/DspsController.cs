using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicTrack.Api.Extensions;
using MusicTrack.Application.Queries;

namespace MusicTrack.Api.Controllers;

public class DspsController : BaseApiController
{
    private readonly IMediator _mediator;

    public DspsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllDspsQuery(), cancellationToken);
        return result.ToHttpResponse();
    }
}
