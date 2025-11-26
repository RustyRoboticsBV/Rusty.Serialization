using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A generic value type converter.
/// </summary>
public abstract class ValueConverter<TargetT, NodeT> : IConverter<TargetT>, IConverter
    where TargetT : struct
    where NodeT : INode
{
    /* Public methods */
    INode IConverter<TargetT>.Convert(TargetT obj, Context context) => Convert(obj, context);

    TargetT IConverter<TargetT>.Deconvert(INode node, Context context)
    {
        if (node is NodeT typed)
            return Deconvert(typed, context);
        throw new Exception($"Cannot interpret nodes of type '{node.GetType()}'.");
    }

    /* Protected methods. */
    protected abstract NodeT Convert(TargetT obj, Context context);
    protected abstract TargetT Deconvert(NodeT node, Context context);

    /// <summary>
    /// Convert an element into a node.
    /// </summary>
    protected INode ConvertElement<T>(Type expectedType, T obj, Context context)
    {
        // Convert obj to node.
        Type valueType = obj.GetType();
        IConverter converter = context.GetConverter(valueType);
        INode node = converter.Convert(obj, context);

        // Wrap inside of a type node if there was a mismatch.
        if (expectedType != valueType)
            node = new TypeNode(context.GetTypeName(valueType), node);

        // Return finished node.
        return node;
    }

    /// <summary>
    /// Deconvert an INode into an element.
    /// </summary>
    protected T DeconvertElement<T>(INode node, Context context)
    {
        Type targetType = ((IConverter)this).TargetType;
        return (T)context.GetConverter(targetType).Deconvert(node, context);
    }
}