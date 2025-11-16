namespace Rusty.Serialization;

/// <summary>
/// A helper class for making strings CSV-safe.
/// </summary>
internal static class StringWriter
{
    /// <summary>
    /// Serialize a string.
    /// </summary>
    public static string Serialize(string text)
    {
        // Handle empty strings.
        if (string.IsNullOrEmpty(text))
            return "";

        // Double " characters.
        text = text.Replace("\"", "\"\"");

        // Enclose in "" if the string contains certain characters.
        if (text.Contains('"')
            || text.Contains(',') || text.Contains(':')
            || text.Contains('[') || text.Contains(']')
            || text.Contains('{') || text.Contains('}')
            || text.Contains('(') || text.Contains(')')
            || text.Contains('\n') || text.Contains('\r'))
        {
            return $"\"{text}\"";
        }

        // Else, return the string as-is.
        else
            return text;
    }
}