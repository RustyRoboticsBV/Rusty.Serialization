using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion.System
{
    /// <summary>
    /// An sbyte converter.
    /// </summary>
    public sealed class SbyteConverter : IntConverter<sbyte>
    {
        /* Protected methods. */
        protected override IntValue ToInt(sbyte obj) => obj;
        protected override sbyte FromInt(IntValue obj) => (sbyte)obj;
    }
}