using MediatR;

namespace I18nApi.Application.Upload.Queries;

public class GetDataQueryHandler: IRequestHandler<GetDataQuery<IList<Dictionary<string, string?>>>>
{
    
}