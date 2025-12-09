using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A generic array converter.
    /// </summary>
    public sealed class ArrayConverter<T> : GenericListConverter<T[], T>
    {
        /* Protected methods. */
        protected override T[] CreateObject(ListNode node, IConverterScheme scheme, NodeTree tree)
            => new T[node.Elements.Length];

        protected override void AssignElements(T[] collection, T[] elements)
        {
            for (int i = 0; i < collection.Length && i < elements.Length; i++)
            {
                collection[i] = elements[i];
            }
        }
    }
}