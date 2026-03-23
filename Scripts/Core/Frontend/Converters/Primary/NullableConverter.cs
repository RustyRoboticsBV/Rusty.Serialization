using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A nullable converter.
    /// </summary>
    public sealed class NullableConverter<ValueT> : Converter<ValueT?, INode>
        where ValueT : struct
    {
        /* Protected methods. */
        protected override INode CreateNode(ValueT? obj, CreateNodeContext context)
        {
            return context.CreateNode(obj);
        }

        protected override ValueT? CreateObject(INode node, CreateObjectContext context)
        {
            return (ValueT?)context.CreateObject(typeof(ValueT), node);
        }
    }
}