namespace Rusty.Serialization;

/// <summary>
/// A helper class unpacking CSV strings.
/// </summary>
internal static class StringParser
{
    /// <summary>
    /// Parses an array string into individual elements.
    /// </summary>
    public static string Parse(string text)
    {
        // Handle empty strings.
        if (string.IsNullOrEmpty(text))
            return "";

        // Handle quoted strings.
        string trimmed = text.Trim();
        if (trimmed.Length > 1 && trimmed.StartsWith('"') && trimmed.EndsWith('"'))
        {
            return trimmed.Substring(1, trimmed.Length - 2)
                .Replace("\"\"", "\"");
        }
        
        // For unquoted strings, just return it as-is.
        return text;
    }
}