using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion.System
{
    /// <summary>
    /// An uint converter.
    /// </summary>
    public sealed class UintConverter : IntConverter<uint>
    {
        /* Protected methods. */
        protected override IntValue ToInt(uint obj) => obj;
        protected override uint FromInt(IntValue obj) => (uint)obj;
    }
}