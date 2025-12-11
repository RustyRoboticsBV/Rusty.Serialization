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
        protected override ListNode CreateNode(CollectionT obj, CreateNodeContext context)
        {
            return new(obj.Count);
        }

        protected override void AssignNode(ListNode node, CollectionT obj, CreateNodeContext context)
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

        protected override void AssignObject(CollectionT obj, ListNode node, CreateObjectContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}