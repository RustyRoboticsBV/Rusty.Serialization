using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An array type converter.
    /// </summary>
    public sealed class ArrayConverter<T> : CompositeReferenceConverter<T[], ListNode>
    {
        protected override ListNode CreateNode(T[] obj)
        {
            return new(obj.Length);
        }

        protected override void AssignNode(ListNode node, T[] obj, AssignNodeContext context)
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

        protected override T[] FixReferences(T[] obj, ListNode node, FixReferencesContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}