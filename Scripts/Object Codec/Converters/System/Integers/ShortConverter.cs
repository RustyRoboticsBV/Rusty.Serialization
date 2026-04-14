using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion.System
{
    /// <summary>
    /// A short converter.
    /// </summary>
    public sealed class ShortConverter : IntConverter<short>
    {
        /* Protected methods. */
        protected override IntValue ToInt(short obj) => obj;
        protected override short FromInt(IntValue obj) => (short)obj;
    }
}