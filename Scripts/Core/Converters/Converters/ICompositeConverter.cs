using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A reference type converter.
    /// </summary>
    public interface ICompositeConverter : IConverter
    {
        /* Public methods. */
        /// <summary>
        /// Create the child nodes.
        /// </summary>
        public void AssignNode(INode node, object obj, CreateNodeContext context);

        /// <summary>
        /// Create the child objects.
        /// </summary>
        public void AssignObject(object obj, INode node, CreateObjectContext context);
    }
}