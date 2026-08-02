using FluentResults;
using Microsoft.AspNetCore.Mvc;
using MusicTrack.Application.Errors;

namespace MusicTrack.Api.Extensions;

public static class ResultExtensions
{
    public static ActionResult ToHttpResponse<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);

        if (result.HasError<NotFoundError>())
            return CreateNotFound(result.Errors);

        return CreateBadRequest(result.Errors);
    }

    public static ActionResult ToHttpResponse(this Result result)
    {
        if (result.IsSuccess)
            return new OkResult();

        if (result.HasError<NotFoundError>())
            return CreateNotFound(result.Errors);

        return CreateBadRequest(result.Errors);
    }

    private static NotFoundObjectResult CreateNotFound(IEnumerable<IError> errors)
    {
        return new NotFoundObjectResult(new
        {
            Errors = errors.Select(e => e.Message)
        });
    }

    private static BadRequestObjectResult CreateBadRequest(IEnumerable<IError> errors)
    {
        return new BadRequestObjectResult(new
        {
            Errors = errors.Select(e => e.Message)
        });
    }
}
