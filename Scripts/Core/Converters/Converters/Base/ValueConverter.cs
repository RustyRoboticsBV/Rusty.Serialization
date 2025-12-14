using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A value type converter.
    /// </summary>
    public abstract class ValueConverter<TargetT, NodeT> : IConverter
        where TargetT : struct
        where NodeT : INode
    {
        /* Public methods. */
        INode IConverter.CreateNode(object obj, CreateNodeContext context) => CreateNode((TargetT)obj, context);
        object IConverter.CreateObject(INode node, CreateObjectContext context) => CreateObject((NodeT)node, context);

        /* Protected methods. */
        protected abstract NodeT CreateNode(TargetT obj, CreateNodeContext context);
        protected abstract TargetT CreateObject(NodeT node, CreateObjectContext context);
    }
}