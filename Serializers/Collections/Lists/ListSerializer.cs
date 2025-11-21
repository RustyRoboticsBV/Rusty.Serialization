using System;
using System.Collections.Generic;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers;

/// <summary>
/// A list serializer.
/// </summary>
public readonly struct ListSerializer<T> : ISerializer<List<T>>
{
    /* Public methods. */
    public INode Serialize(List<T> value, Registry context, bool addTypeLabel = false)
    {
        // Handle null.
        if (value == null)
            return new NullNode();

        // Serialize elements.
        ISerializer elementSerializer = context.GetSerializer(typeof(T));

        INode[] elements = new INode[value.Count];
        for (int i = 0; i < value.Count; i++)
        {
            elements[i] = elementSerializer.Serialize(value[i], context);
        }

        return new ListNode(elements);
    }

    public List<T> Deserialize(INode node, Registry context)
    {
        if (node is NullNode @null)
            return null;
        if (node is TypeNode type)
            return Deserialize(type.Object, context);
        if (node is ListNode list)
        {
            // Deserialize elements.
            ISerializer elementSerializer = context.GetSerializer(typeof(T));

            var elements = list.Elements;

            List<T> result = new();
            for (int i = 0; i < elements.Length; i++)
            {
                result.Add((T)elementSerializer.Deserialize(elements[i], context));
            }

            return result;
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}