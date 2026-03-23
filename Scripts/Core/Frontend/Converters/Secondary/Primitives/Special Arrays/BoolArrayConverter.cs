using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A bool array converter.
    /// </summary>
    public sealed class BoolArrayConverter : TypedBitmaskConverter<bool[]>
    {
        /* Protected methods. */
        protected override BitmaskValue ToBitmask(bool[] obj) => (BitmaskValue)obj;
        protected override bool[] FromBitmask(BitmaskValue value) => value.value;
    }
}