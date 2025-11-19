using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers;

/// <summary>
/// An array serializer.
/// </summary>
public readonly struct ArraySerializer<T> : ISerializer<T[]>
{
    /* Public methods. */
    public INode Serialize(T[] value, Registry context)
    {
        // Handle null.
        if (value == null)
            return new NullNode();

        // Serialize elements.
        ISerializer elementSerializer = context.GetSerializer(typeof(T));

        INode[] elements = new INode[value.Length];
        for (int i = 0; i < value.Length; i++)
        {
            elements[i] = elementSerializer.Serialize(value[i], context);
        }

        return new ListNode(elements);
    }

    public T[] Deserialize(INode node, Registry context)
    {
        if (node is NullNode @null)
            return null;
        else if (node is ListNode list)
        {
            // Deserialize elements.
            ISerializer elementSerializer = context.GetSerializer(typeof(T));

            var elements = list.Elements;

            T[] result = new T[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                result[i] = (T)elementSerializer.Deserialize(elements[i], context);
            }

            return result;
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}