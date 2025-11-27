using System;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A System.Range converter.
    /// </summary>
    public sealed class RangeConverter : VectorConverter<Range, Index>
    {
        /* Protected properties. */
        protected override int Length => 2;

        /* Protected methods. */
        protected override Index GetAt(ref Range obj, int index)
        {
            switch (index)
            {
                case 0: return obj.Start;
                case 1: return obj.End;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Range obj, int index, Index value)
        {
            switch (index)
            {
                case 0: obj = new(value, obj.End); break;
                case 1: obj = new(obj.Start, value); break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}