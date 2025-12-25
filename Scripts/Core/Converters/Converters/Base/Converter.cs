using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A base class for all converter.
    /// </summary>
    public abstract class Converter<TargetT, NodeT> : IConverter
        where NodeT : INode
    {
        /* Public methods. */
        public void CollectTypes(INode node, CollectTypesContext context) => CollectTypes((NodeT)node, context);
        INode IConverter.CreateNode(object obj, CreateNodeContext context) => CreateNode((TargetT)obj, context);
        object IConverter.CreateObject(INode node, CreateObjectContext context) => CreateObject((NodeT)node, context);

        /* Protected methods. */
        /// <summary>
        /// Collect the type of a node, as well as the types of members.
        /// </summary>
        protected virtual void CollectTypes(NodeT node, CollectTypesContext context) { }
        /// <summary>
        /// Create a node from an object.
        /// </summary>
        protected abstract NodeT CreateNode(TargetT obj, CreateNodeContext context);
        /// <summary>
        /// Create an object from a node.
        /// </summary>
        protected abstract TargetT CreateObject(NodeT node, CreateObjectContext context);
    }
}