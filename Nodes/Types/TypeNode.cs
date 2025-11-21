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
    public readonly TypeName Value => value;
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
            throw new InvalidOperationException("obj was null.");
        return $"({value}){obj.Serialize()}";
    }

    public static TypeNode Parse(string text)
    {
        // Trim trailing whitespaces.
        string trimmed = text?.Trim();

        try
        {
            // Enforce parentheses.
            int closeIndex = trimmed.IndexOf(')');
            if (!trimmed.StartsWith('(') || closeIndex == -1)
                throw new Exception("Missing parentheses.");

            // Get text after parentheses and parse it.
            string objStr = trimmed.Substring(closeIndex + 1).Trim();
            INode obj = ParseUtility.ParseValue(objStr);

            // Get text between parentheses and trim it.
            string contents = trimmed.Substring(1, closeIndex - 1).Trim();

            // Return type node.
            return new(contents, obj);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Could not parse string '{text}' as a type:\n\n{ex.Message}");
        }
    }
}