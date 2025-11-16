#if GODOT
using System;
using StringName = Godot.StringName;
#endif

namespace Rusty.Serialization.Nodes;

/// <summary>
/// A text serializer node.
/// </summary>
public struct String : INode
{
    /* Fields. */
    private readonly string value;

    /* Constructors. */
    private String(string value)
    {
        this.value = value;
    }

    /* Conversion operators. */
    public static implicit operator String(string value) => new(value);
#if GODOT
    public static implicit operator String(StringName value) => new(value);
#endif

    public static implicit operator string(String node) => node.value;
#if GODOT
    public static implicit operator StringName(String value) => new(value);
#endif

    /* Public methods. */
    public readonly string Serialize()
    {
        return '"' + value.Replace("\"", "\"\"") + '"';
    }

    public static String Deserialize(string text)
    {
        string trimmed = text?.Trim();
        if (trimmed != null && trimmed.StartsWith('"') && trimmed.EndsWith('"'))
        {
            return new(trimmed.Substring(1, trimmed.Length - 2).Replace("\"\"", "\""));
        }
        throw new ArgumentException($"Could not parse string '{text}' as a string.");
    }
}