using I18nApi.Application.Upload.Queries;
using I18nApi.Contracts.Data;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using I18nApi.Application.Upload.Common;

namespace I18nApi.Api.Controllers;

[Route("[controller]")]
public class DataController: BaseApiController
{
    private readonly IMapper _mapper;
    public DataController(ILogger<BaseApiController> logger, IMediator mediator, IMapper mapper) : base(logger, mediator)
    {
        _mapper = mapper;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> Search([FromQuery]GetDataRequest request)
    {
        GetDataQuery<ErrorOr<DataQueryResult<IList<Dictionary<string, string?>>>>> query =
            _mapper.Map<GetDataQuery<ErrorOr<DataQueryResult<IList<Dictionary<string, string?>>>>>>(request);

        ErrorOr<DataQueryResult<IList<Dictionary<string, string?>>>> result = await Mediator.Send(query);
        
        return result.Match(Ok, Problem);
    }
}
