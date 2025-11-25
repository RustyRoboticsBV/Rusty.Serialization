using System;
using System.Collections;
using System.Collections.Generic;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A generic IEnumerable converter.
/// </summary>
public abstract class EnumerableRefConverter<CollectionT, ElementT, NodeT> : ReferenceConverter<CollectionT, NodeT>
    where CollectionT : class, IEnumerable
    where NodeT : INode
{
    /* Protected methods. */
    protected override NodeT Convert(CollectionT obj, Context context)
    {
        // Convert the elements to nodes.
        List<INode> elementNodes = new();
        foreach (ElementT element in obj)
        {
            elementNodes.Add(ConvertElement(element, context));
        }

        // Create the node.
        return CreateNode(elementNodes.ToArray());
    }

    protected override CollectionT Deconvert(NodeT node, Context context)
    {
        IEnumerable<INode> elementNodes = GetElements(node);
        List<ElementT> elements = new();
        foreach (INode elementNode in elementNodes)
        {
            ElementT element = DeconvertElement(elementNode, context);
            elements.Add(element);
        }
        return CreateObject(elements.ToArray());
    }

    /// <summary>
    /// Create an INode from an array of element nodes.
    /// </summary>
    protected abstract NodeT CreateNode(INode[] elements);

    /// <summary>
    /// Create an object from an array of elements.
    /// </summary>
    protected abstract CollectionT CreateObject(ElementT[] elements);

    /// <summary>
    /// Get the INode elements from an INode.
    /// </summary>
    protected abstract IEnumerable<INode> GetElements(NodeT node);

    /* Private methods. */
    /// <summary>
    /// Convert an element to an INode.
    /// </summary>
    private INode ConvertElement(ElementT obj, Context context)
    {
        return context.GetConverter(obj.GetType()).Convert(obj, context);
    }

    /// <summary>
    /// Deconvert an INode into an element.
    /// </summary>
    private ElementT DeconvertElement(INode node, Context context)
    {
        Type targetType = ((IConverter)this).TargetType;
        return (ElementT)context.GetConverter(targetType).Deconvert(node, context);
    }
}