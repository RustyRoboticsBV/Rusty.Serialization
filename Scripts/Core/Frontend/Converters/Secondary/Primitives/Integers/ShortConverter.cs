using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A short converter.
    /// </summary>
    public sealed class ShortConverter : TypedIntConverter<short>
    {
        /* Protected methods. */
        protected override IntValue ToInt(short obj) => (IntValue)obj;
        protected override short FromInt(IntValue value) => (short)value;
    }
}