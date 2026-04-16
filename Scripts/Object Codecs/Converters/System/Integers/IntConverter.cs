using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion.System
{
    /// <summary>
    /// An int converter.
    /// </summary>
    public sealed class IntConverter : IntConverter<int>
    {
        /* Protected methods. */
        protected override IntValue ToInt(int obj) => obj;
        protected override int FromInt(IntValue obj) => (int)obj;
    }
}