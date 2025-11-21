using System;

namespace Rusty.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
public class SerializableAttribute : Attribute
{
    /* Fields. */
    private readonly string typeCode;

    /* Public properties. */
    public string TypeCode => typeCode;

    /* Constructors. */
    public SerializableAttribute(string typeCode)
    {
        this.typeCode = typeCode;
    }
}