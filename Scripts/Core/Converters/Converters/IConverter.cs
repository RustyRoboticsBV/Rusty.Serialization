using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A converter between objects and nodes.
    /// </summary>
    public interface IConverter
    {
        /* Public methods. */
        /// <summary>
        /// Create a node from an object.
        /// </summary>
        public INode CreateNode(object obj);

        /// <summary>
        /// Create an object from a node. References child nodes are not handled yet.
        /// </summary>
        public object CreateObject(INode node, CreateObjectContext context);
    }
}