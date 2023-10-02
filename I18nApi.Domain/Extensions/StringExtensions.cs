using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace I18nApi.Domain.Extensions;

public static class StringExtensions
{
    public static string ToNormalizedString(this string str)
    {
        string normalizedText = str.Normalize(NormalizationForm.FormD);
        StringBuilder result = new();

        for (int i = 0; i < normalizedText.Length; i++)
        {
            char c = normalizedText[i];
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                result.Append(c);
            }
        }
        
        normalizedText = Regex.Replace(result.ToString(), "[^a-zA-Z0-9]", "_");
        
        normalizedText = normalizedText.ToLower();

        return normalizedText;
    }
}