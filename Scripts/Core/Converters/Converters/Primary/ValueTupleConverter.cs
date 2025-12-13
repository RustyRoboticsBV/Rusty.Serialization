using System.Runtime.CompilerServices;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A value tuple type converter.
    /// </summary>
    public sealed class ValueTupleConverter<T> : ValueConverter<T, ListNode>
        where T : struct, ITuple
    {
        /* Protected methods. */
        protected override ListNode CreateNode(T obj)
        {
            throw new System.NotImplementedException();
        }

        protected override T CreateObject(ListNode node, CreateObjectContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}