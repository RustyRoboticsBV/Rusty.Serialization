using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A generic reference type converter.
/// </summary>
public abstract class ReferenceConverter<TargetT, NodeT> : IConverter<TargetT>, IConverter
    where TargetT : class
    where NodeT : INode
{
    /* Public methods */
    INode IConverter<TargetT>.Convert(TargetT obj, Context context) => Convert(obj, context);

    TargetT IConverter<TargetT>.Deconvert(INode node, Context context)
    {
        if (node is NullNode)
            return null;
        if (node is NodeT typed)
            return Deconvert(typed, context);
        throw new Exception($"Cannot interpret nodes of type '{node.GetType()}'.");
    }

    /* Protected methods. */
    protected abstract NodeT Convert(TargetT obj, Context context);
    protected abstract TargetT Deconvert(NodeT node, Context context);
}