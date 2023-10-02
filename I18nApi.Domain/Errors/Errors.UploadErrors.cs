using ErrorOr;

namespace I18nApi.Domain.Errors;

public static partial class Errors
{
    public static class UploadErrors
    {
        private const string Prefix = "UploadErrors";
        public static Error NoFile(string message) => Error.Validation($"{Prefix}.NoFile", message);
        public static Error UnsupportedType(string message) => Error.Validation($"{Prefix}.UnsupportedType", message);

    }
}