using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Rusty.Serialization.Nodes;

internal static class ParseUtility
{
    /// <summary>
    /// Takes a comma-delimited string and parses it into a list of serializer node hierarchies.
    /// </summary>
    public static List<INode> Parse(string text)
    {
        // Handle null.
        if (text == null)
            throw new ArgumentException("Cannot parse null.");

        // Handle empty list.
        if (text.Length == 0)
            return [];

        // Parse text.
        List<INode> result = new();
        bool inChar = false;
        bool inString = false;
        int depth = 0;
        int start = 0;

        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];

            if (inString)
            {
                if (c == '"')
                {
                    bool isDoubled = i + 1 < text.Length && text[i + 1] == '"';
                    if (isDoubled)
                        i++;
                    else
                        inString = false;
                }
            }

            else if (inChar && c == '\'')
                inChar = false;

            else if (c == '"')
                inString = true;

            else if (c == '\'')
                inChar = true;

            else if (c == ',' && depth == 0)
            {
                string segment = text.Substring(start, i - start).Trim();
                result.Add(Deserialize(segment));
                start = i + 1;
            }

            else
            {
                switch (c)
                {
                    case '[':
                    case '{':
                    //case '(':
                    case '<':
                        depth++;
                        break;
                    case ']':
                    case '}':
                    //case ')':
                    case '>':
                        depth--;
                        break;
                }
            }
        }

        string final = text.Substring(start).Trim();
        result.Add(Deserialize(final));

        return result;
    }

    /// <summary>
    /// Parse a trimmed string and return a serializer node.
    /// </summary>
    private static INode Deserialize(string text)
    {
        if (text == null)
            throw new ArgumentException("Cannot parse null.");

        if (text.StartsWith("#"))
            return Color.Deserialize(text);
        else if (text.StartsWith('\'') && text.EndsWith('\''))
            return Character.Deserialize(text);
        else if (text.StartsWith('"') && text.EndsWith('"'))
            return String.Deserialize(text);
        else if (text.StartsWith('[') && text.EndsWith(']'))
            return List.Deserialize(text);
        else if (text.StartsWith('{') && text.EndsWith('}'))
        { return null; /* TODO: dictionaries */ }
        else if (text.StartsWith('<') && text.EndsWith('>'))
        { return null; /* TODO: objects */ }
        else if (text.ToLower() == "true" || text.ToLower() == "false")
            return Boolean.Deserialize(text);
        else if (decimal.TryParse(text, CultureInfo.InvariantCulture, out decimal d))
        {
            if (text.Contains('.'))
                return Float.Deserialize(text);
            else
                return Integer.Deserialize(text);
        }
        else
            throw new ArgumentException($"Invalid string '{text}'");
    }
}