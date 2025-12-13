using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A composite value type converter.
    /// </summary>
    public abstract class CompositeValueConverter<TargetT, NodeT> : ValueConverter<TargetT, NodeT>, ICompositeConverter
        where TargetT : struct
        where NodeT : INode
    {
        /* Public methods. */
        void ICompositeConverter.AssignNode(INode node, object obj, AssignNodeContext context) => AssignNode((NodeT)node, (TargetT)obj, context);
        object ICompositeConverter.FixReferences(object obj, INode node, FixReferencesContext context) => FixReferences((TargetT)obj, (NodeT)node, context);

        /* Protected methods. */
        protected abstract void AssignNode(NodeT node, TargetT obj, AssignNodeContext context);
        protected abstract TargetT FixReferences(TargetT obj, NodeT node, FixReferencesContext context);
    }
}