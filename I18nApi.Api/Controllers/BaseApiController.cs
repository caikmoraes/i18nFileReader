using ErrorOr;
using I18nApi.Api.Common.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace I18nApi.Api.Controllers;

[ApiController]

public class BaseApiController : ControllerBase
{
    protected ILogger<BaseApiController> Logger;
    protected IMediator Mediator;

    public BaseApiController(ILogger<BaseApiController> logger, IMediator mediator)
    {
        Logger = logger;
        Mediator = mediator;
    }

    protected IActionResult Problem(List<Error> errors)
    {
        Logger.LogError("One or more errors occured: {errors}", errors);
        if (errors.Count is 0)
        {
            return Problem();
        }
        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }
        HttpContext.Items[HttpContextItemKeys.Errors] = errors;

        return Problem(errors[0]);
    }

    private IActionResult Problem(Error error)
    {
        int statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
        return Problem(statusCode: statusCode, title: error.Description);
    }

    private IActionResult ValidationProblem(List<Error> errors)
    {
        ModelStateDictionary modelStateDictionary = new();
        foreach (Error error in errors)
        {
            modelStateDictionary.AddModelError(
                error.Code,
                error.Description
            );
        }
        return ValidationProblem(modelStateDictionary);
    }
}