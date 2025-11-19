using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers;

/// <summary>
/// A string serializer.
/// </summary>
public readonly struct EnumSerializer<T> : ISerializer<T>
    where T : Enum
{
    /* Fields. */
    private readonly TypeName typeCode;

    /* Constructors. */
    public EnumSerializer(TypeName typeCode)
    {
        Type type = typeof(T);
        this.typeCode = typeCode;
        System.Console.WriteLine(this.typeCode);
    }

    /* Public methods. */
    public INode Serialize(T value, Registry context)
    {
        return new EnumNode(typeCode, value.ToString());
    }

    public T Deserialize(INode node, Registry context)
    {
        if (node is EnumNode enumNode)
        {
            if (enumNode.typeName != typeCode)
                throw new ArgumentException($"Cannot deserialize enum '{enumNode.typeName}' as '{typeCode}'.");

            if (Enum.TryParse(typeof(T), enumNode.memberName, false, out object parsed))
            {
                string[] names = Enum.GetNames(typeof(T));
                bool isNamed = Array.IndexOf(names, enumNode.memberName) >= 0;
                if (isNamed)
                    return (T)parsed;
            }
            else
                throw new ArgumentException($"'{enumNode.memberName}' is not a valid member of enum '{typeof(T).Name}'.");

            object value = Enum.Parse(typeof(T), enumNode.memberName);
            return (T)value;
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}