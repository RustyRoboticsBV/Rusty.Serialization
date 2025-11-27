using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A long converter.
    /// </summary>
    public sealed class LongConverter : ValueConverter<long, IntNode>
    {
        /* Protected methods. */
        protected override IntNode Convert(long obj, Context context) => new(obj);
        protected override long Deconvert(IntNode node, Context context) => (long)node.Value;
    }
}