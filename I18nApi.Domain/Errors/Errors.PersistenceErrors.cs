using ErrorOr;

namespace I18nApi.Domain.Errors;

public static partial class Errors
{
    public static class PersistenceErrors
    {
        private const string Prefix = "PersistenceErrors";
        public static Error PersistenceFailure(string message) => Error.Failure($"{Prefix}.PersistenceFailure", message);
    }
}