using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion.System
{
    /// <summary>
    /// An ushort converter.
    /// </summary>
    public sealed class UshortConverter : IntConverter<ushort>
    {
        /* Protected methods. */
        protected override IntValue ToInt(ushort obj) => obj;
        protected override ushort FromInt(IntValue obj) => (ushort)obj;
    }
}