using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Codecs
{
    /// <summary>
    /// A base class for serializer/deserializer classes.
    /// </summary>
    public abstract class Codec
    {
        /* Public methods. */
        /// <summary>
        /// Serialize a node tree into a string.
        /// </summary>
        public abstract string Serialize(NodeTree tree, Settings prettyPrint);

        /// <summary>
        /// Deserialize a string into a node tree.
        /// </summary>
        public abstract NodeTree Parse(string serialized);
    }
}