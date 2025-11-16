using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A helper class for parsing tuple strings.
/// </summary>
internal static class TupleParser
{
    /// <summary>
    /// Parses a tuple string into individual elements.
    /// </summary>
    public static (string, string) Parse(string text)
    {
        // Handle empty strings.
        if (string.IsNullOrEmpty(text))
            return ("", "");

        // Trim enclosing brackets.
        text = text.Trim();
        if (text.StartsWith('(') && text.EndsWith(')'))
            text = text.Substring(1, text.Length - 2);

        // Parse CSV.
        List<string> fields = FieldsParser.Parse(text);

        // Return result.
        (string, string) result = new();
        if (fields.Count > 0)
            result.Item1 = fields[0];
        if (fields.Count > 1)
            result.Item2 = fields[1];
        return result;
    }
}