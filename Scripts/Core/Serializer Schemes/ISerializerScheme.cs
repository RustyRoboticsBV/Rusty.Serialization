using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Serializers
{
    /// <summary>
    /// A serialization scheme.
    /// </summary>
    public interface ISerializerScheme
    {
        /* Public methods. */
        /// <summary>
        /// Serialize an INode hierarchy.
        /// </summary>
        public string Serialize(INode node);

        /// <summary>
        /// Parse a string into an INode hierarchy.
        /// </summary>
        public INode Parse(string serialized);
    }
}