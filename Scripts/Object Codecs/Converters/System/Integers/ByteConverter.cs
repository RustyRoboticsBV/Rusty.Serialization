using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion.System
{
    /// <summary>
    /// A byte converter.
    /// </summary>
    public sealed class ByteConverter : IntConverter<byte>
    {
        /* Protected methods. */
        protected override IntValue ToInt(byte obj) => obj;
        protected override byte FromInt(IntValue obj) => (byte)obj;
    }
}