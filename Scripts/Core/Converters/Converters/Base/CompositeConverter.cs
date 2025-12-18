using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A composite type converter.
    /// </summary>
    public abstract class CompositeConverter<TargetT, NodeT> : Converter<TargetT, NodeT>, ICompositeConverter
        where NodeT : INode
    {
        /* Public methods. */
        void ICompositeConverter.AssignNode(INode node, object obj, AssignNodeContext context) => AssignNode((NodeT)node, (TargetT)obj, context);
        object ICompositeConverter.FixReferences(object obj, INode node, FixReferencesContext context) => FixReferences((TargetT)obj, (NodeT)node, context);

        /* Protected methods. */
        /// <summary>
        /// Create the child nodes.
        /// </summary>
        protected abstract void AssignNode(NodeT node, TargetT obj, AssignNodeContext context);
        /// <summary>
        /// Fix the the missing references of the created object.
        /// </summary>
        protected abstract TargetT FixReferences(TargetT obj, NodeT node, FixReferencesContext context);
    }
}