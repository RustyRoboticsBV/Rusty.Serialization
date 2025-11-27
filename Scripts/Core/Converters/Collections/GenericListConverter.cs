using System.Collections;
using System.Collections.Generic;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A generic IEnumerable converter.
    /// </summary>
    public abstract class GenericListConverter<CollectionT, ElementT> : ReferenceConverter<CollectionT, ListNode>
        where CollectionT : class, IEnumerable
    {
        /* Protected methods. */
        protected override ListNode Convert(CollectionT obj, Context context)
        {
            // Convert the elements to nodes.
            List<INode> elementNodes = new();
            foreach (ElementT element in obj)
            {
                elementNodes.Add(ConvertElement(typeof(ElementT), element, context));
            }

            // Create the node.
            return new(elementNodes.ToArray());
        }

        protected override CollectionT Deconvert(ListNode node, Context context)
        {
            ElementT[] elements = new ElementT[node.Elements.Length];
            for (int i = 0; i < node.Elements.Length; i++)
            {
                elements[i] = DeconvertElement<ElementT>(node.Elements[i], context);
            }
            return CreateObject(elements);
        }

        /// <summary>
        /// Create an object from an array of elements.
        /// </summary>
        protected abstract CollectionT CreateObject(ElementT[] elements);
    }
}