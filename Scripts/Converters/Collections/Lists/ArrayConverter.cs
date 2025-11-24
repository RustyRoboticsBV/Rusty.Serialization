using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A generic array converter.
/// </summary>
public sealed class ArrayConverter<T> : ReferenceConverter<T[], ListNode>
{
    /* Protected methods. */
    protected override ListNode Convert(T[] obj, Context context)
    {
        INode[] elementNodes = new INode[obj.Length];
        for (int i = 0; i < obj.Length; i++)
        {
            elementNodes[i] = ConvertElement(obj[i], context);
        }
        return new(elementNodes);
    }

    protected sealed override T[] Deconvert(ListNode node, Context context)
    {
        T[] array = new T[node.Elements.Length];
        for (int i = 0; i < node.Elements.Length; i++)
        {
            array[i] = DeconvertElement(node.Elements[i], context);
        }
        return array;
    }

    /* Private methods. */
    private INode ConvertElement(T obj, Context context)
    {
        return context.GetConverter(obj.GetType()).Convert(obj, context);
    }

    private T DeconvertElement(INode node, Context context)
    {
        Type targetType = ((IConverter)this).TargetType;
        return (T)context.GetConverter(targetType).Deconvert(node, context);
    }
}