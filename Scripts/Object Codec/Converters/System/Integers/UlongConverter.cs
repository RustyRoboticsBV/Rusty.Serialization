using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion.System
{
    /// <summary>
    /// An ulong converter.
    /// </summary>
    public sealed class UlongConverter : IntConverter<ulong>
    {
        /* Protected methods. */
        protected override IntValue ToInt(ulong obj) => obj;
        protected override ulong FromInt(IntValue obj) => (ulong)obj;
    }
}