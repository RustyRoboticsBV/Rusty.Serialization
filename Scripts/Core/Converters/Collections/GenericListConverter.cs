using System.Collections;
using System.Collections.Generic;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A generic IEnumerable converter.
    /// </summary>
    public abstract class GenericListConverter<CollectionT, ElementT> : ReferenceConverter<CollectionT, ListNode>
        where CollectionT : class, IEnumerable
    {
        /* Protected methods. */
        protected override ListNode ConvertRef(CollectionT obj, IConverterScheme scheme)
        {
            // Convert the elements to nodes.
            List<INode> elementNodes = new();
            foreach (ElementT element in obj)
            {
                elementNodes.Add(ConvertNested(typeof(ElementT), element, scheme));
            }

            // Create the node.
            return new(elementNodes.ToArray());
        }

        protected override CollectionT DeconvertRef(ListNode node, IConverterScheme scheme)
        {
            ElementT[] elements = new ElementT[node.Elements.Length];
            for (int i = 0; i < node.Elements.Length; i++)
            {
                elements[i] = DeconvertNested<ElementT>(node.Elements[i], scheme);
            }
            return CreateObject(elements);
        }

        /// <summary>
        /// Create an object from an array of elements.
        /// </summary>
        protected abstract CollectionT CreateObject(ElementT[] elements);
    }
}