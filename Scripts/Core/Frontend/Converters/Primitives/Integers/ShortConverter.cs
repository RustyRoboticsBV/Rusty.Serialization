using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An short converter.
    /// </summary>
    public sealed class ShortConverter : IntBaseConverter<short>
    {
        /* Protected methods. */
        protected override IntValue ToInt(short obj) => obj;
        protected override short FromInt(IntValue obj) => (short)obj;
    }
}