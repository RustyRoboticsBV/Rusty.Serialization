using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion.System
{
    /// <summary>
    /// A bool converter.
    /// </summary>
    public sealed class BoolConverter : BoolConverter<bool>
    {
        /* Protected methods. */
        protected override BoolValue ToBool(bool obj) => obj;
        protected override bool FromBool(BoolValue obj) => (bool)obj;
    }
}