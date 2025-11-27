using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A ulong converter.
    /// </summary>
    public sealed class UlongConverter : ValueConverter<ulong, IntNode>
    {
        /* Protected methods. */
        protected override IntNode Convert(ulong obj, Context context) => new(obj);
        protected override ulong Deconvert(IntNode node, Context context) => (ulong)node.Value;
    }
}