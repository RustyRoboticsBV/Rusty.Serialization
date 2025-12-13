using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A reference type converter.
    /// </summary>
    public abstract class ReferenceConverter<TargetT, NodeT> : IConverter
        where TargetT : class
        where NodeT : INode
    {
        /* Public methods. */
        INode IConverter.CreateNode(object obj) => CreateNode((TargetT)obj);
        object IConverter.CreateObject(INode node, CreateObjectContext context) => CreateObject((NodeT)node, context);

        /* Protected methods. */
        protected abstract NodeT CreateNode(TargetT obj);
        protected abstract TargetT CreateObject(NodeT node, CreateObjectContext context);
    }
}