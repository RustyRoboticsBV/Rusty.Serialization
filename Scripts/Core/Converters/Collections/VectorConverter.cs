using Rusty.Serialization.Nodes;
using System;

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
        protected override ListNode Convert(VectorT obj, Context context)
        {
            INode[] nodes = new INode[Length];
            for (int i = 0; i < Length; i++)
            {
                nodes[i] = ConvertElement(typeof(ElementT), GetAt(ref obj, i), context);
            }
            return new(nodes);
        }

        protected override VectorT Deconvert(ListNode node, Context context)
        {
            if (node.Elements.Length != Length)
                throw new ArgumentException($"Expected a list node with length '{Length}', received '{node.Elements.Length}'.");
            VectorT vector = new();
            for (int i = 0; i < Length; i++)
            {
                SetAt(ref vector, i, DeconvertElement<ElementT>(node.Elements[i], context));
            }
            return vector;
        }

        protected abstract ElementT GetAt(ref VectorT obj, int index);
        protected abstract void SetAt(ref VectorT obj, int index, ElementT value);
    }
}