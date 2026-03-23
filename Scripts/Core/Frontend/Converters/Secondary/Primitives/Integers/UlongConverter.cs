using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A ulong converter.
    /// </summary>
    public sealed class UlongConverter : TypedIntConverter<ulong>
    {
        /* Protected methods. */
        protected override IntValue ToInt(ulong obj) => (IntValue)obj;
        protected override ulong FromInt(IntValue value) => (ulong)value;
    }
}