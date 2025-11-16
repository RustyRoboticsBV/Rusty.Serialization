using System;
using System.Linq;
using System.Reflection;

namespace Rusty.Serialization.Nodes;

using SysList = System.Collections.Generic.List<INode>;
#if GODOT
using GdList = Godot.Collections.Array<INode>;
using static System.Net.Mime.MediaTypeNames;
#endif

/// <summary>
/// A list serializer node.
/// </summary>
public struct List : INode
{
    /* Fields. */
    private readonly INode[] elements;

    /* Constructors. */
    private List(INode[] elements)
    {
        this.elements = elements;
    }

    /* Conversion operators. */
    public static implicit operator List(INode[] elements) => new(elements);
    public static implicit operator List(SysList elements) => new([.. elements]);
#if GODOT
    public static implicit operator List(GdList elements) => new([.. elements]);
#endif

    public static implicit operator INode[](List node) => node.elements;
    public static implicit operator SysList(List node) => [.. node.elements];
#if GODOT
    public static implicit operator GdList(List node) => [.. node.elements];
#endif

    /* Public methods. */
    public override readonly string ToString()
    {
        string str = "array: ";
        for (int i = 0; i < elements.Length; i++)
        {
            str += "\n-" + elements[i].ToString().Replace("\n", "\n ");
        }
        return str;
    }

    public readonly string Serialize()
    {
        return "[" + string.Join(",", elements.Select(e => e.Serialize())) + "]";
    }

    public static List Deserialize(string text)
    {
        // Trim trailing whitespaces.
        string trimmed = text?.Trim();

        try
        {
            // Enforce square brackets.
            if (trimmed.StartsWith('[') & trimmed.EndsWith(']'))
            {
                string contents = trimmed.Substring(1, trimmed.Length - 2);
                return new(ParseUtility.Parse(contents).ToArray());
            }
            else
                throw new Exception();
        }
        catch
        {
            throw new ArgumentException($"Could not parse string '{text}' as a list.");
        }
    }
}