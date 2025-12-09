using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Collections.Generic.List converter.
    /// </summary>
    public sealed class ListConverter<T> : GenericListConverter<List<T>, T>
    {
        /* Protected methods. */
        protected override List<T> CreateObject(ListNode node, IConverterScheme scheme, ParsingTable table) => new();

        protected override void AssignElements(List<T> collection, T[] elements)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                collection.Add(elements[i]);
            }
        }
    }
}