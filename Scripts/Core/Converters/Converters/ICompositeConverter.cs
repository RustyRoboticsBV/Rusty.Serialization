using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
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
        public void AssignNode(INode node, object obj, AssignNodeContext context);

        /// <summary>
        /// Fix the missing references of the created object.
        /// </summary>
        public object AssignObject(object obj, INode node, AssignObjectContext context);
    }
}