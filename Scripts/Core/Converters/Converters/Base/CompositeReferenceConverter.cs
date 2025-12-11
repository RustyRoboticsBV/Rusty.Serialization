using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A composite reference type converter.
    /// </summary>
    public abstract class CompositeReferenceConverter<TargetT, NodeT> : ReferenceConverter<TargetT, NodeT>, ICompositeConverter
        where TargetT : class
        where NodeT : INode
    {
        /* Public methods. */
        void ICompositeConverter.AssignNode(INode node, object obj, CreateNodeContext context) => AssignNode((NodeT)node, (TargetT)obj, context);
        void ICompositeConverter.AssignObject(object obj, INode node, CreateObjectContext context) => AssignObject((TargetT)obj, (NodeT)node, context);

        /* Protected methods. */
        protected abstract void AssignNode(NodeT node, TargetT obj, CreateNodeContext context);
        protected abstract void AssignObject(TargetT obj, NodeT node, CreateObjectContext context);
    }
}