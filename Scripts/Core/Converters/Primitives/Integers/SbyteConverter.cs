using Rusty.Serialization.Core.Contexts;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
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