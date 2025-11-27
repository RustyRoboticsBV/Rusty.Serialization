using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A short converter.
    /// </summary>
    public sealed class ShortConverter : ValueConverter<short, IntNode>
    {
        /* Protected methods. */
        protected override IntNode Convert(short obj, Context context) => new(obj);
        protected override short Deconvert(IntNode node, Context context) => (short)node.Value;
    }
}