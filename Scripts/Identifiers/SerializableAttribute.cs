using System;

namespace Rusty.Serialization;

/// <summary>
/// An attribute that can be used to control a type's name when serialized with a type label.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
public class SerializableAttribute : Attribute
{
    /* Fields. */
    private readonly string name;

    /* Public properties. */
    public string Name => name;

    /* Constructors. */
    public SerializableAttribute(string name)
    {
        this.name = name;
    }
}