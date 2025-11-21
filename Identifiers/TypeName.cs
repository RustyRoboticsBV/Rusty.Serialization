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
        if (name.Contains('(') || name.Contains(')'))
            throw new ArgumentException("Type names may not contain '(' or ')'.");
        this.name = name;
    }

    /* Conversion operators. */
    public static implicit operator string(TypeName identifier) => identifier.name;
    public static implicit operator TypeName(string name) => new TypeName(name);

    /* Public method */
    public override string ToString() => name;
}
