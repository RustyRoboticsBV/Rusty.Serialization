using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A float converter.
    /// </summary>
    public sealed class FloatConverter : Converter<float, RealNode>
    {
        /* Protected methods. */
        protected override RealNode CreateNode(float obj, CreateNodeContext context) => new(obj);
        protected override float CreateObject(RealNode node, CreateObjectContext context) => float.Parse(node.Value);
    }
}