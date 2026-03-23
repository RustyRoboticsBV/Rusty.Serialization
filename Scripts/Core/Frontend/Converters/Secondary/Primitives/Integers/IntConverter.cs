using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An int converter.
    /// </summary>
    public sealed class IntConverter : TypedIntConverter<int>
    {
        /* Protected methods. */
        protected override IntValue ToInt(int obj) => (IntValue)obj;
        protected override int FromInt(IntValue value) => (int)value;
    }
}