using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An int converter.
    /// </summary>
    public sealed class IntConverter : IntBaseConverter<int>
    {
        protected override IntValue ToInt(int obj) => obj;

        protected override int FromInt(IntValue obj) => (int)obj;
    }
}