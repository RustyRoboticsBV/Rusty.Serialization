using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion.System
{
    /// <summary>
    /// A float converter.
    /// </summary>
    public sealed class FloatConverter : FloatConverter<float>
    {
        /* Protected methods. */
        protected override FloatValue ToFloat(float obj) => obj;
        protected override float FromFloat(FloatValue obj) => (float)obj;
    }
}