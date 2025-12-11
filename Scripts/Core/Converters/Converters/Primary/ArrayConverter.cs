using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An array type converter.
    /// </summary>
    public sealed class ArrayConverter<T> : CompositeReferenceConverter<T[], ListNode>
    {
        protected override ListNode CreateNode(T[] obj, CreateNodeContext context)
        {
            return new(obj.Length);
        }

        protected override void AssignNode(ListNode node, T[] obj, CreateNodeContext context)
        {
            for (int i = 0; i < obj.Length; i++)
            {
                node.Elements[i] = context.CreateNode(typeof(T), obj[i]);
            }
        }

        protected override T[] CreateObject(ListNode node, CreateObjectContext context)
        {
            throw new System.NotImplementedException();
        }

        protected override void AssignObject(T[] obj, ListNode node, CreateObjectContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}