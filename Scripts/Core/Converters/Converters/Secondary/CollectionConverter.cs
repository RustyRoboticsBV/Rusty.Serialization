using Rusty.Serialization.Core.Nodes;
using System.Collections.Generic;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An ICollection converter.
    /// </summary>
    public class CollectionConverter<CollectionT, ElementT> : CompositeReferenceConverter<CollectionT, ListNode>
        where CollectionT : class, ICollection<ElementT>, new()
    {
        /* Protected methods. */
        protected override ListNode CreateNode(CollectionT obj, CreateNodeContext context)
        {
            return new(obj.Count);
        }

        protected override void AssignNode(ListNode node, CollectionT obj, AssignNodeContext context)
        {
            int index = 0;
            foreach (ElementT element in obj)
            {
                node.Elements[index] = context.CreateNode(typeof(CollectionT), element);
                index++;
            }
        }

        protected override CollectionT CreateObject(ListNode node, CreateObjectContext context)
        {
            CollectionT obj = new();
            for (int i = 0; i < node.Elements.Length; i++)
            {
                obj.Add(context.CreateObject<ElementT>(node));
            }
            return obj;
        }

        protected override CollectionT FixReferences(CollectionT obj, ListNode node, FixReferencesContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}