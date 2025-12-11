using Rusty.Serialization.Core.Nodes;
using System.Collections.Generic;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An IList converter.
    /// </summary>
    public class ListConverter<CollectionT, ElementT> : CollectionConverter<CollectionT, ElementT>
        where CollectionT : class, IList<ElementT>, new()
    {
        protected override void AssignNode(ListNode node, CollectionT obj, CreateNodeContext context)
        {
            for (int i = 0; i < obj.Count; i++)
            {
                node.Elements[i] = context.CreateNode(typeof(CollectionT), obj[i]);
            }
        }

        protected override CollectionT CreateObject(ListNode node, CreateObjectContext context)
        {
            throw new System.NotImplementedException();
        }

        protected override void AssignObject(CollectionT obj, ListNode node, CreateObjectContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}