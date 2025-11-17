using System;
using System.Collections.Generic;
using System.Linq;

namespace Rusty.Serialization.Nodes;

using SysDict = Dictionary<INode, INode>;
#if GODOT
using GdDict = Godot.Collections.Dictionary<INode, INode>;
#endif

/// <summary>
/// A dictionary serializer node.
/// </summary>
public struct Dictionary : INode
{
    /* Fields. */
    private readonly KeyValuePair<INode, INode>[] pairs;

    /* Constructors. */
    public Dictionary(KeyValuePair<INode, INode>[] pairs)
    {
        this.pairs = pairs ?? [];
    }

    public Dictionary(IDictionary<INode, INode> dict)
    {
        pairs = new KeyValuePair<INode, INode>[dict.Count];
        int index = 0;
        foreach (var pair in dict)
        {
            pairs[index] = pair;
            index++;
        }
    }

    /* Conversion operators. */
    public static implicit operator Dictionary(SysDict dict) => new(dict);
#if GODOT
    public static implicit operator Dictionary(GdDict dict) => new(dict);
#endif

    public static implicit operator SysDict(Dictionary node) => new(node.pairs);
#if GODOT
    public static implicit operator GdDict(Dictionary node)
    {
        var dict = new GdDict();
        foreach (var pair in node.pairs)
            dict[pair.Key] = pair.Value;
        return dict;
    }
#endif

    /* Public methods. */
    public override readonly string ToString()
    {
        if (pairs.Length == 0)
            return "dictionary: (empty)";

        string str = "dictionary:";
        foreach (var kv in pairs)
        {
            string keyStr = kv.Key.ToString().Replace("\n", "\n ");
            string valStr = kv.Value.ToString().Replace("\n", "\n ");
            str += $"\n- {keyStr} => {valStr}";
        }
        return str;
    }

    public readonly string Serialize()
    {
        if (pairs.Length == 0)
            return "{}";

        return "{" + string.Join(",", pairs.Select(
            kv => kv.Key.Serialize() + ":" + kv.Value.Serialize()
        )) + "}";
    }

    public static Dictionary Deserialize(string text)
    {
        string trimmed = text?.Trim() ?? throw new ArgumentException("Cannot parse null.");

        try
        {
            // Enforce curly braces.
            if (!trimmed.StartsWith('{') || !trimmed.EndsWith('}'))
                throw new Exception("Missing curly braces.");

            // Get text between curly braces and trim it.
            string contents = trimmed.Substring(1, trimmed.Length - 2).Trim();

            // Split into terms.
            List<string> parsed = ParseUtility.Split(contents);

            // Parse terms.
            var pairs = new KeyValuePair<INode, INode>[parsed.Count];
            for (int i = 0; i < parsed.Count; i++)
            {
                // Split into key and value.
                List<string> pairStrs = ParseUtility.Split(parsed[i], ':');
                if (pairStrs.Count != 2)
                    throw new Exception("Malformed key-value pair.");

                // Parse keys and values.
                INode key = ParseUtility.ParseValue(pairStrs[0].Trim());
                INode value = ParseUtility.ParseValue(pairStrs[1].Trim());
                pairs[i] = new(key, value);
            }

            return new Dictionary(pairs);

        }
        catch
        {
            throw new ArgumentException($"Could not parse string '{text}' as a dictionary.");
        }
    }
}
