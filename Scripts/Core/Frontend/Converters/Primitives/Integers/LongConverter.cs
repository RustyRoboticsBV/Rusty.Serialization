using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An long converter.
    /// </summary>
    public sealed class LongConverter : IntBaseConverter<long>
    {
        /* Protected methods. */
        protected override IntValue ToInt(long obj) => obj;
        protected override long FromInt(IntValue obj) => (long)obj;
    }
}