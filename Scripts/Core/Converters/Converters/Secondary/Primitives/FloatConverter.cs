using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A float converter.
    /// </summary>
    public sealed class FloatConverter : Converter<float, RealNode>
    {
        /* Protected methods. */
        protected override RealNode CreateNode(float obj, CreateNodeContext context) => new(PeterO.Numbers.EDecimal.FromDouble(obj));
        protected override float CreateObject(RealNode node, CreateObjectContext context) => (float)node.Value;
    }
}