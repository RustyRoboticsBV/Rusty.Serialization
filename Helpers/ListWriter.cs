using System.Collections.Generic;
using System.Text;

namespace Rusty.Serialization;

/// <summary>
/// A helper class for serializing arrays, lists, etc.
/// </summary>
internal static class ListWriter
{
    /// <summary>
    /// Serialize a collection.
    /// </summary>
    public static string Serialize<T>(IList<T> list)
        where T : ISerializer
    {
        if (list == null || list.Count == 0)
            return "[]";

        StringBuilder sb = new();
        bool first = true;
        sb.Append('[');
        foreach (T item in list)
        {
            if (!first)
                sb.Append(",");
            sb.Append(item?.Serialize() ?? "");
            first = false;
        }
        sb.Append(']');
        return sb.ToString();
    }
}