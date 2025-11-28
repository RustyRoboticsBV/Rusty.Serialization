using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Serializers
{
    /// <summary>
    /// A serializer/deserializer that can convert between an INode and serialized, text-based data.
    /// </summary>
    public interface ISerializer
    {
        /* Public methods */
        /// <summary>
        /// Serialize a node.
        /// </summary>
        public string Serialize(INode node, ISerializerScheme scheme);

        /// <summary>
        /// Deserialize a string into an INode hierarchy.
        /// </summary>
        public INode Parse(string serialized, ISerializerScheme scheme);
    }

    /// <summary>
    /// A serializer/deserializer that can convert between an INode and serialized, text-based data.
    /// </summary>
    public interface ISerializer<T> : ISerializer
        where T : INode
    {
        /* Public methods */
        /// <summary>
        /// Serialize a node.
        /// </summary>
        public string Serialize(T node, ISerializerScheme scheme);

        /// <summary>
        /// Deserialize a string into an INode hierarchy.
        /// </summary>
        public new T Parse(string serialized, ISerializerScheme scheme);
    }
}