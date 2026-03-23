using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A nullable converter.
    /// </summary>
    public sealed class NullableConverter<ValueT> : TypedConverter<ValueT?, INode>
        where ValueT : struct
    {
        /* Protected methods. */
        protected override INode CreateNode2(ValueT? obj, CreateNodeContext context)
        {
            return context.CreateNode(obj);
        }

        protected override ValueT? CreateObject2(INode node, CreateObjectContext context)
        {
            return (ValueT?)context.CreateObject(typeof(ValueT), node);
        }
    }
}