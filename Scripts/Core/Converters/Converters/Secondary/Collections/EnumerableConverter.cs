using Rusty.Serialization.Core.Nodes;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An IEnumerable converter.
    /// </summary>
    public abstract class EnumerableConverter<EnumerableT, ElementT, NodeT> : CompositeConverter<EnumerableT, NodeT>
        where EnumerableT : IEnumerable<ElementT>
        where NodeT : ICollectionNode
    {
        /* Protected methods. */
        protected override NodeT CreateNode(EnumerableT obj, CreateNodeContext context)
        {
            return (NodeT)Activator.CreateInstance(typeof(NodeT), GetCount(obj));
        }

        protected override EnumerableT CreateObject(NodeT node, CreateObjectContext context)
        {
            ElementT[] elements = new ElementT[node.Count];
            for (int i = 0; i < node.Count; i++)
            {
                elements[i] = context.CreateObject<ElementT>(node.GetValueAt(i));
            }
            return CreateObjectFromElements(elements);
        }

        protected override EnumerableT FixReferences(EnumerableT obj, NodeT node, FixReferencesContext context)
        {
            ElementT[] elements = new ElementT[node.Count];
            int index = 0;
            foreach (ElementT element in obj)
            {
                elements[index] = (ElementT)context.FixReferences(element, node.GetValueAt(index));
                index++;
            }
            return CreateObjectFromElements(elements);
        }

        /// <summary>
        /// Get the number of elements in a collection.
        /// </summary>
        protected virtual int GetCount(EnumerableT obj)
        {
            int count = 0;
            foreach (ElementT element in obj)
            {
                count++;
            }
            return count;
        }

        /// <summary>
        /// Create a object from an array of elements.
        /// </summary>
        protected virtual EnumerableT CreateObjectFromElements(ICollection<ElementT> elements)
        {
            return (EnumerableT)Activator.CreateInstance(typeof(EnumerableT), elements);
        }
    }
}