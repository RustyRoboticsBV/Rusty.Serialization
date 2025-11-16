using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A helper class for parsing dictionary strings.
/// </summary>
internal static class DictionaryParser
{
    /// <summary>
    /// Parses an array string into individual elements.
    /// </summary>
    public static List<(string, string)> Parse(string text)
    {
        // Handle empty strings.
        if (string.IsNullOrEmpty(text))
            return [];

        // Trim enclosing brackets.
        text = text.Trim();
        if (text.StartsWith('{') && text.EndsWith('}'))
            text = text.Substring(1, text.Length - 2);

        // Parse CSV.
        List<string> terms = FieldsParser.Parse(text);

        // Parse key-value pairs.
        List<(string, string)> result = new();
        foreach (string term in terms)
        {
            List<string> pair = FieldsParser.Parse(term, ':');

            string key = "";
            if (pair.Count > 0)
                key = StringParser.Parse(pair[0]);

            string value = "";
            if (pair.Count > 1)
                value = StringParser.Parse(pair[1]);

            result.Add((key, value));
        }

        return result;
    }
}