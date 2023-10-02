using ErrorOr;
using I18nApi.Application.Upload.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace I18nApi.Application.Upload.Commands;

public record UploadFileCommand(
    IFormFile? File
): IRequest<ErrorOr<UploadResult>>;