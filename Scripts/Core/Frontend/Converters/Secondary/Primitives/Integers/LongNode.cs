using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A long converter.
    /// </summary>
    public sealed class LongConverter : TypedIntConverter<long>
    {
        /* Protected methods. */
        protected override IntValue ToInt(long obj) => (IntValue)obj;
        protected override long FromInt(IntValue value) => (long)value;
    }
}