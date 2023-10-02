using ErrorOr;
using I18nApi.Application.Upload.Commands;
using I18nApi.Application.Upload.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace I18nApi.Api.Controllers;

[Route("[controller]")]
public class UploadController : BaseApiController
{
    public UploadController(ILogger<BaseApiController> logger, IMediator mediator) : base(logger, mediator)
    {
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> Upload(IFormFile? file)
    {
        Logger.LogInformation("Hadling upload endpoint");
        UploadFileCommand command = new(file);

        ErrorOr<UploadResult> uploadResult = await Mediator.Send(command);

        return uploadResult.Match(Ok, Problem);
    }
}