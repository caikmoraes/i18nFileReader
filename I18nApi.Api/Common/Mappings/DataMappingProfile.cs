using ErrorOr;
using I18nApi.Application.Upload.Queries;
using I18nApi.Contracts.Data;
using Mapster;

namespace I18nApi.Api.Common.Mappings;

public class DataMappingProfile: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetDataRequest, GetDataQuery<ErrorOr<IList<Dictionary<string, string?>>>>>();
    }
}