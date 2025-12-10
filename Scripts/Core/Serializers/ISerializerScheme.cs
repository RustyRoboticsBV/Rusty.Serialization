using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Serializers
{
    /// <summary>
    /// A serialization scheme.
    /// </summary>
    public interface ISerializerScheme
    {
        /* Public properties. */
        /// <summary>
        /// Whether or not pretty print has been enabled.
        /// </summary>
        public bool PrettyPrint { get; set; }
        /// <summary>
        /// The tab string used for pretty printing.
        /// </summary>
        public string Tab { get; set; }

        /* Public methods. */
        /// <summary>
        /// Serialize an INode hierarchy.
        /// </summary>
        public string Serialize(INode node);

        /// <summary>
        /// Parse a string into a node tree.
        /// </summary>
        public NodeTree ParseAsTree(string serialized);

        /// <summary>
        /// Parse a string into an INode hierarchy.
        /// </summary>
        public INode ParseAsNode(string serialized);
    }
}