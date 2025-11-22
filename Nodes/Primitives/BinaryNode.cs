using System;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// A binary string serializer node.
/// </summary>
public readonly struct BinaryNode : INode
{
    /* Fields. */
    private readonly byte[] value;

    /* Public properties. */
    public readonly byte[] Value => value;

    /* Constructors. */
    public BinaryNode(byte[] value)
    {
        this.value = value;
    }

    /* Public methods. */
    public override readonly string ToString()
    {
        return "binary: " + value;
    }

    public readonly string Serialize()
    {
        return $"0x{Convert.ToHexString(value)}";
    }

    public static BinaryNode Parse(string text)
    {
        // Remove whitespaces.
        string trimmed = text?.Trim();

        try
        {
            // Empty strings are not allowed.
            if (string.IsNullOrEmpty(trimmed))
                throw new ArgumentException("Empty string.");

            // Enforce 0x prefix.
            if (!trimmed.StartsWith("0x"))
                throw new ArgumentException("Missing 0x prefix.");

            // Get contents.
            string contents = trimmed.Substring(2);

            // Enforce even length.
            if (contents.Length % 2 != 0)
                throw new ArgumentException("Binary literals must have an even length.");

            // Parse as byte array.
            byte[] bytes = Convert.FromHexString(contents);

            return new(bytes);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Could not parse string '{text}' as a binary:\n{ex.Message}");
        }
    }
}