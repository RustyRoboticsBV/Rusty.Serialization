using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A uint converter.
    /// </summary>
    public sealed class UintConverter : ValueConverter<uint, IntNode>
    {
        /* Protected methods. */
        protected override IntNode Convert(uint obj, Context context) => new(obj);
        protected override uint Deconvert(IntNode node, Context context) => (uint)node.Value;
    }
}