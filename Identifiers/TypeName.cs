using System;

namespace Rusty.Serialization;

public struct TypeName
{
    /* Fields. */
    private readonly string name;

    /* Constructors. */
    public TypeName(Type type) : this(type.FullName) { }

    public TypeName(string name)
    {
        if (Validate(name))
            this.name = name;
        else
            throw new ArgumentException($"'{name}' is not a valid C# type name.");
    }

    /* Conversion operators. */
    public static implicit operator string(TypeName identifier) => identifier.name;
    public static implicit operator TypeName(string name) => new TypeName(name);

    /* Public method */
    public override string ToString() => name;

    /* Private methods. */
    private static bool Validate(string name)
    {
        // No empty or null strings allowed.
        if (string.IsNullOrEmpty(name))
            return false;

        // Check each character.
        bool isFirst = true;
        for (int i = 0; i < name.Length; i++)
        {
            char c = name[i];

            // First character must be letter or underscore.
            if (isFirst)
            {
                isFirst = false;
                if (!(char.IsLetter(c) || c == '_'))
                    return false;
            }
            else
            {
                // Dots mark namespace members, plus signs mark nested types.
                if (c == '.' || c == '+')
                    isFirst = true;

                // Any other character that is not a leter, digit or underscore is illegal.
                else if (!(char.IsLetterOrDigit(c) || c == '_'))
                    return false;
            }
        }

        return true;
    }
}
