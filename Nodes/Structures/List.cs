using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rusty.Serialization.Nodes;

using SysList = System.Collections.Generic.List<INode>;
using StrList = System.Collections.Generic.List<string>;
#if GODOT
using GdList = Godot.Collections.Array<INode>;
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

    public List(IList<INode> list) : this(list.ToArray()) { }

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
            if (!trimmed.StartsWith('[') || !trimmed.EndsWith(']'))
                throw new Exception("Missing square brackets.");

            // Get text between square brackets and trim it.
            string contents = trimmed.Substring(1, trimmed.Length - 2);

            // Split into terms.
            StrList terms = ParseUtility.Split(contents);

            // Create child nodes.
            INode[] nodes = new INode[terms.Count];
            for (int i = 0; i < terms.Count; i++)
            {
                nodes[i] = ParseUtility.ParseValue(terms[i]);
            }

            // Create list node.
            return new(nodes);
        }
        catch
        {
            throw new ArgumentException($"Could not parse string '{text}' as a elements.");
        }
    }
}