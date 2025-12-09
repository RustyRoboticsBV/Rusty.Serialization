using System.Collections;
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
        protected override ListNode CreateNode(CollectionT obj, IConverterScheme scheme, SymbolTable table)
        {
            int count = 0;
            foreach (ElementT elementT in obj)
            {
                count++;
            }

            return new(count);
        }

        protected override void AssignNode(ref ListNode node, CollectionT obj, IConverterScheme scheme, SymbolTable table)
        {
            // Convert the elements to nodes.
            int index = 0;
            foreach (ElementT element in obj)
            {
                INode elementNode = ConvertNested(typeof(ElementT), element, scheme, table);
                node.Elements[index] = elementNode;
                elementNode.Parent = node;
                index++;
            }
        }

        protected override void AssignObject(CollectionT obj, ListNode node, IConverterScheme scheme, NodeTree tree)
        {
            // Deconvert elements.
            ElementT[] elements = new ElementT[node.Elements.Length];
            for (int i = 0; i < node.Elements.Length; i++)
            {
                elements[i] = DeconvertNested<ElementT>(node.Elements[i], scheme, tree);
            }

            // Fill the collection.
            AssignElements(obj, elements);
        }

        /// <summary>
        /// Create an object from an array of elements.
        /// </summary>
        protected abstract void AssignElements(CollectionT collection, ElementT[] elements);
    }
}