using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A uint converter.
    /// </summary>
    public sealed class UintConverter : TypedIntConverter<uint>
    {
        /* Protected methods. */
        protected override IntValue ToInt(uint obj) => (IntValue)obj;
        protected override uint FromInt(IntValue value) => (uint)value;
    }
}