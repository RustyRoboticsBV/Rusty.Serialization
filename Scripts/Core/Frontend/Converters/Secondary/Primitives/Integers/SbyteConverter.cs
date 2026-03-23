using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A sbyte converter.
    /// </summary>
    public sealed class SbyteConverter : TypedIntConverter<sbyte>
    {
        /* Protected methods. */
        protected override IntValue ToInt(sbyte obj) => (IntValue)obj;
        protected override sbyte FromInt(IntValue value) => (sbyte)value;
    }
}