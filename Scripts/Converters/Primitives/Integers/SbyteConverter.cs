using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// An sbyte converter.
    /// </summary>
    public sealed class SbyteConverter : ValueConverter<sbyte, IntNode>
    {
        /* Protected methods. */
        protected override IntNode Convert(sbyte obj, Context context) => new(obj);
        protected override sbyte Deconvert(IntNode node, Context context) => (sbyte)node.Value;
    }
}