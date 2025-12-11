using System.Runtime.CompilerServices;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A reference tuple type converter.
    /// </summary>
    public sealed class ReferenceTupleConverter<T> : CompositeReferenceConverter<T, ListNode>
        where T : class, ITuple, new()
    {
        /* Protected methods. */
        protected override T CreateObject(ListNode node, CreateObjectContext context)
        {
            throw new System.NotImplementedException();
        }

        protected override void AssignNode(ListNode node, T obj, CreateNodeContext context)
        {
            throw new System.NotImplementedException();
        }

        protected override ListNode CreateNode(T obj, CreateNodeContext context)
        {
            throw new System.NotImplementedException();
        }

        protected override void AssignObject(T obj, ListNode node, CreateObjectContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}