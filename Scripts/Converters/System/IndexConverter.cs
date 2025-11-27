using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A System.Index converter.
    /// </summary>
    public sealed class IndexConverter : ValueConverter<Index, IntNode>
    {
        /* Protected methods. */
        protected override IntNode Convert(Index obj, Context context) => new(obj.Value);
        protected override Index Deconvert(IntNode node, Context context) => (int)node.Value;
    }
}