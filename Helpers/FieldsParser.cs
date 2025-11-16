using System.Collections.Generic;
using System.Text;

namespace Rusty.Serialization;

/// <summary>
/// A helper class for parsing a line of CSV into its fields.
/// </summary>
internal static class FieldsParser
{
    /// <summary>
    /// Parse a single CSV line into fields.
    /// </summary>
    public static List<string> Parse(string text, char delimiter = ',')
    {
        // Handle empty strings.
        List<string> result = [];
        if (string.IsNullOrEmpty(text))
            return result;

        StringBuilder sb = new();
        bool inQuotes = false;

        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];

            if (inQuotes)
            {
                if (c == '"')
                {
                    if (i + 1 < text.Length && text[i + 1] == '"')
                    {
                        sb.Append('"');
                        i++;
                    }
                    else
                        inQuotes = false;
                }
                else
                    sb.Append(c);
            }
            else
            {
                if (c == '"')
                    inQuotes = true;
                else if (c == delimiter)
                {
                    result.Add(sb.ToString());
                    sb.Clear();
                }
                else
                    sb.Append(c);
            }
        }
        
        result.Add(sb.ToString());

        return result;
    }
}