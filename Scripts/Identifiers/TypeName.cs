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
        this.name = name;
    }

    /* Conversion operators. */
    public static implicit operator string(TypeName identifier) => identifier.name;
    public static implicit operator TypeName(string name) => new TypeName(name);

    /* Public method */
    public override string ToString() => name;
    public override int GetHashCode() => name?.GetHashCode() ?? 0;
}
