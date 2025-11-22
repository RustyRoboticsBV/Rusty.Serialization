using System;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// A type label serializer node.
/// </summary>
public readonly struct TypeNode : INode
{
    /* Fields. */
    private readonly TypeName value;
    private readonly INode obj;

    /* Public properties. */
    public readonly TypeName TypeCode => value;
    public readonly INode Object => obj;

    /* Constructors. */
    public TypeNode(TypeName value, INode obj)
    {
        this.value = value;
        this.obj = obj;
    }

    /* Public methods. */
    public override readonly string ToString()
    {
        if (obj == null)
            return "type: " + value + " (null)";

        string objStr = obj.ToString().Replace("\n", "\n ");
        return "type: " + value + "\n " + objStr;
    }

    public readonly string Serialize()
    {
        if (obj == null)
            throw new InvalidOperationException("value node was null.");
        return $"({value}){obj.Serialize()}";
    }

    public static TypeNode Parse(string text)
    {
        // Remove whitespaces.
        string trimmed = text?.Trim();

        try
        {
            // Empty strings are not allowed.
            if (string.IsNullOrEmpty(trimmed))
                throw new ArgumentException("Empty string.");

            // Enforce parentheses.
            int closeIndex = trimmed.IndexOf(')');
            if (!trimmed.StartsWith('(') || closeIndex == -1)
                throw new ArgumentException("Missing parentheses.");

            // Get text between parentheses and trim it.
            string name = trimmed.Substring(1, closeIndex - 1).Trim();

            // Get text after parentheses and parse it.
            string value = trimmed.Substring(closeIndex + 1).Trim();
            INode valueNode = null;
            if (value.Length > 0)
                valueNode = ParseUtility.ParseValue(value);

            // Return type node.
            return new(name, valueNode);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Could not parse string '{text}' as a type:\n{ex.Message}");
        }
    }
}