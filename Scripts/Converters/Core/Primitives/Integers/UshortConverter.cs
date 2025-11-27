using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A ushort converter.
    /// </summary>
    public sealed class UshortConverter : ValueConverter<ushort, IntNode>
    {
        /* Protected methods. */
        protected override IntNode Convert(ushort obj, Context context) => new(obj);
        protected override ushort Deconvert(IntNode node, Context context) => (ushort)node.Value;
    }
}