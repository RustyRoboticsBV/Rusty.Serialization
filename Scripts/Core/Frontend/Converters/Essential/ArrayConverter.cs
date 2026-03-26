using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An array converter.
    /// </summary>
    public sealed class ArrayConverter<T> : Converter
    {
        public override void CollectTypes(object obj, CollectTypesContext context)
        {
            T[] array = TryCast<T[]>(obj);
            for (int i = 0; i < array.Length; i++)
            {
                context.Collect(typeof(T), array[i]);
            }
        }

        public override INode CreateNode(object obj, CreateNodeContext context)
        {
            ListNode node = new ListNode();
            return node;
        }
    }
}