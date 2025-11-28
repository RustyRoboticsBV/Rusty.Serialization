using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Serializers
{
    /// <summary>
    /// A base class for all serializers.
    /// </summary>
    public abstract class Serializer<T> : ISerializer<T>
        where T : INode
    {
        public string Serialize(INode node, ISerializerScheme scheme)
        {
            if (node is T typed)
                return Serialize(typed, scheme);
            throw new ArgumentException($"Cannot serialize nodes of type '{node}'.");
        }

        public abstract string Serialize(T node, ISerializerScheme scheme);

        INode ISerializer.Parse(string serialized, ISerializerScheme scheme)
        {
            return Parse(serialized, scheme);
        }

        public abstract T Parse(string serialized, ISerializerScheme scheme);
    }
}