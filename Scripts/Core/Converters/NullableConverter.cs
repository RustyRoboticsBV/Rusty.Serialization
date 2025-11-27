using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A nullable type converter.
    /// </summary>
    public sealed class NullableConverter<TargetT> : IConverter<TargetT?>
        where TargetT : struct
    {
        /* Public methods */
        INode IConverter<TargetT?>.Convert(TargetT? obj, Context context)
        {
            if (obj == null)
                return new NullNode();
            else
                return ConvertElement(obj.Value.GetType(), obj.Value, context);
        }

        TargetT? IConverter<TargetT?>.Deconvert(INode node, Context context)
        {
            if (node is NullNode)
                return null;
            else
                return DeconvertElement<TargetT>(node, context);
            throw new Exception($"Cannot interpret nodes of type '{node.GetType()}'.");
        }

        /* Private methods. */
        /// <summary>
        /// Convert an element into a node.
        /// </summary>
        private INode ConvertElement<T>(Type expectedType, T obj, Context context)
        {
            // Convert obj to node.
            Type valueType = obj.GetType();
            IConverter converter = context.GetConverter(valueType);
            INode node = converter.Convert(obj, context);

            // Wrap inside of a type node if there was a mismatch.
            if (expectedType != valueType && valueType != null)
                node = new TypeNode(context.GetTypeName(valueType), node);

            // Return finished node.
            return node;
        }

        /// <summary>
        /// Deconvert an INode into an element.
        /// </summary>
        private T DeconvertElement<T>(INode node, Context context)
        {
            // Unwrap type nodes.
            if (node is TypeNode @type)
                return DeconvertElement<T>(@type.Value, context);

            // Get converter.
            Type targetType = ((IConverter)this).TargetType;
            IConverter converter = context.GetConverter(targetType);

            // Deconvert.
            return (T)converter.Deconvert(node, context);
        }
    }
}