using System;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// A type label serializer node.
/// </summary>
public readonly struct TypeNode : INode
{
    /* Fields. */
    private readonly string name;
    private readonly INode value;

    /* Public properties. */
    public readonly string Name => name;
    public readonly INode Value => value;

    /* Constructors. */
    public TypeNode(string name, INode value)
    {
        this.name = name;
        this.value = value;
    }

    /* Public methods. */
    public override readonly string ToString()
    {
        if (value == null)
            return "type: " + name + " (null)";

        string objStr = value.ToString().Replace("\n", "\n ");
        return "type: " + name + "\n   => " + objStr;
    }

    public readonly string Serialize()
    {
        string name = this.name.Trim();
        Validate(name);

        if (value == null)
            throw new InvalidOperationException("name was null.");
        return $"({this.name}){value.Serialize()}";
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

            // Get text between parentheses, validate and trim it.
            string name = trimmed.Substring(1, closeIndex - 1).Trim();
            Validate(name);

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

    /* Private methods. */
    private static void Validate(string name)
    {
        for (int i = 0; i < name.Length; i++)
        {
            char c = name[i];
            if (c < '!' && c > '~' || c == '(' || c == ')')
                throw new ArgumentException($"Disallowed character '{c}' in type '{name}'.");
        }
    }
}