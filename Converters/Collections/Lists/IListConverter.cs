using Rusty.Serialization.Nodes;
using System;
using System.Collections;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A generic IList converter.
/// </summary>
public abstract class IListConverter<ListType, ElementType> : ReferenceConverter<ListType, ListNode>
    where ListType : class, IList
{
    /* Protected methods. */
    protected abstract ListType Instantiate();

    protected override ListNode Convert(ListType obj, Context context)
    {
        INode[] elementNodes = new INode[obj.Count];
        for (int i = 0; i < obj.Count; i++)
        {
            elementNodes[i] = ConvertElement((ElementType)obj[i], context);
        }
        return new(elementNodes);
    }

    protected override ListType Deconvert(ListNode node, Context context)
    {
        ListType obj = Instantiate();
        for (int i = 0; i < node.Elements.Length; i++)
        {
            obj.Add(DeconvertElement(node.Elements[i], context));
        }
        return obj;
    }

    protected virtual INode ConvertElement(ElementType obj, Context context)
    {
        return context.GetConverter(obj.GetType()).Convert(obj, context);
    }

    protected virtual ElementType DeconvertElement(INode node, Context context)
    {
        Type targetType = ((IConverter)this).TargetType;
        return (ElementType)context.GetConverter(targetType).Deconvert(node, context);
    }
}