using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A ushort converter.
    /// </summary>
    public sealed class UshortConverter : TypedIntConverter<ushort>
    {
        /* Protected methods. */
        protected override IntValue ToInt(ushort obj) => (IntValue)obj;
        protected override ushort FromInt(IntValue value) => (ushort)value;
    }
}