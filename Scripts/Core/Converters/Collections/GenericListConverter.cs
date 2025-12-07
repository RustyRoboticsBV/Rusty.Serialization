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
        protected override ListNode ConvertRef(CollectionT obj, IConverterScheme scheme, SymbolTable table)
        {
            // Create node.
            int count = 0;
            foreach (ElementT elementT in obj)
            {
                count++;
            }

            ListNode node = new(count);

            // Convert the elements to nodes.
            int index = 0;
            foreach (ElementT element in obj)
            {
                INode elementNode = ConvertNested(typeof(ElementT), element, scheme, table);
                node.Elements[index] = elementNode;
                elementNode.Parent = node;
                index++;
            }

            return node;
        }

        protected override CollectionT DeconvertRef(ListNode node, IConverterScheme scheme, NodeTree tree)
        {
            ElementT[] elements = new ElementT[node.Elements.Length];
            for (int i = 0; i < node.Elements.Length; i++)
            {
                elements[i] = DeconvertNested<ElementT>(node.Elements[i], scheme, tree);
            }
            return CreateObject(elements);
        }

        /// <summary>
        /// Create an object from an array of elements.
        /// </summary>
        protected abstract CollectionT CreateObject(ElementT[] elements);
    }
}