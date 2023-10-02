using Microsoft.Extensions.Logging;

namespace I18nApi.Infrastructure.Persistence;

public abstract class BaseRepository
{
    protected readonly ILogger<BaseRepository> Logger;

    protected BaseRepository(ILogger<BaseRepository> logger)
    {
        Logger = logger;
    }
}