using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A double converter.
    /// </summary>
    public sealed class DoubleConverter : Converter<double, RealNode>
    {
        /* Protected methods. */
        protected override RealNode CreateNode(double obj, CreateNodeContext context) => new(obj);
        protected override double CreateObject(RealNode node, CreateObjectContext context) => double.Parse(node.Value);
    }
}