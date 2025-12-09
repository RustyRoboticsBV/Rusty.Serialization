using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A generic vector struct converter.
    /// </summary>
    public abstract class VectorConverter<VectorT, ElementT> : ValueConverter<VectorT, ListNode>
        where VectorT : struct
    {
        /* Protected properties. */
        protected abstract int Length { get; }

        /* Protected methods. */
        protected override ListNode ConvertValue(VectorT obj, IConverterScheme scheme, SymbolTable table)
        {
            ListNode node = new(Length);
            for (int i = 0; i < Length; i++)
            {
                node.Elements[i] = ConvertNested(typeof(ElementT), GetAt(ref obj, i), scheme, table);
            }
            return node;
        }

        protected override VectorT DeconvertValue(ListNode node, IConverterScheme scheme, ParsingTable table)
        {
            if (node.Elements.Length != Length)
                throw new ArgumentException($"Expected a list node with length '{Length}', received '{node.Elements.Length}'.");
            VectorT vector = new();
            for (int i = 0; i < Length; i++)
            {
                SetAt(ref vector, i, DeconvertNested<ElementT>(node.Elements[i], scheme, table));
            }
            return vector;
        }

        protected abstract ElementT GetAt(ref VectorT obj, int index);
        protected abstract void SetAt(ref VectorT obj, int index, ElementT value);
    }
}