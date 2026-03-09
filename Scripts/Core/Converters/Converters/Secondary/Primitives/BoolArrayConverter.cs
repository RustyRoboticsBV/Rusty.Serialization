using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A bool array converter.
    /// </summary>
    public sealed class BoolArrayConverter : Converter<bool[], BitmaskNode>
    {
        /* Protected methods. */
        protected override BitmaskNode CreateNode(bool[] obj, CreateNodeContext context) => new BitmaskNode(obj);
        protected override bool[] CreateObject(BitmaskNode node, CreateObjectContext context) => node.Value;
    }
}