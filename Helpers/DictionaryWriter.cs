using System.Collections.Generic;
using System.Text;

namespace Rusty.Serialization;

/// <summary>
/// A helper class for serializing dictionaries.
/// </summary>
internal static class DictionaryWriter
{
    /// <summary>
    /// Serialize an enumerable.
    /// </summary>
    public static string Serialize<T, U>(IDictionary<T, U> dictionary)
        where T : ISerializer
        where U : ISerializer
    {
        if (dictionary == null || dictionary.Count == 0)
            return "{}";

        StringBuilder sb = new();
        bool first = true;
        sb.Append('{');
        foreach (KeyValuePair<T, U> item in dictionary)
        {
            if (!first)
                sb.Append(",");
            sb.Append(item.Key?.Serialize() ?? "");
            sb.Append(':');
            sb.Append(item.Value?.Serialize() ?? "");
            first = false;
        }
        sb.Append('}');
        return sb.ToString();
    }
}