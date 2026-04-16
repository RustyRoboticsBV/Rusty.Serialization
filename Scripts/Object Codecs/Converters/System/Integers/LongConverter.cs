using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion.System
{
    /// <summary>
    /// A long converter.
    /// </summary>
    public sealed class LongConverter : IntConverter<long>
    {
        /* Protected methods. */
        protected override IntValue ToInt(long obj) => obj;
        protected override long FromInt(IntValue obj) => (long)obj;
    }
}