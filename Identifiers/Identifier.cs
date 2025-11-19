using System;

namespace Rusty.Serialization;

public struct Identifier
{
    /* Fields. */
    private readonly string name;

    /* Constructors. */
    public Identifier(string name)
    {
        if (Validate(name))
            this.name = name;
        else
            throw new ArgumentException($"'{name}' is not a valid C# identifier name.");
    }

    /* Conversion operators. */
    public static implicit operator string(Identifier identifier) => identifier.name;
    public static implicit operator Identifier(string name) => new Identifier(name);

    /* Public method */
    public override string ToString() => name;

    /* Private methods. */
    private static bool Validate(string name)
    {
        // No empty or null strings allowed.
        if (string.IsNullOrEmpty(name))
            return false;

        // First character must be a letter or underscore.
        char first = name[0];
        if (!(char.IsLetter(first) || first == '_'))
            return false;

        // Remaining characters can be letters, digits, or underscores.
        for (int i = 1; i < name.Length; i++)
        {
            char c = name[i];
            if (!(char.IsLetterOrDigit(c) || c == '_'))
                return false;
        }

        return true;
    }
}
