using System.Collections.Generic;
namespace Rusty.Serialization;

/// <summary>
/// A helper class for parsing array strings.
/// </summary>
internal static class ListParser
{
    /// <summary>
    /// Parses an array string into individual elements.
    /// </summary>
    public static List<string> Parse(string text)
    {
        // Handle empty strings.
        if (string.IsNullOrEmpty(text))
            return [];

        // Trim enclosing brackets.
        text = text.Trim();
        if (text.StartsWith('[') && text.EndsWith(']'))
            text = text.Substring(1, text.Length - 2);

        // Parse CSV.
        return FieldsParser.Parse(text);
    }
}