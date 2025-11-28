using Rusty.Serialization.Core.Contexts;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A bool converter.
    /// </summary>
    public sealed class BoolConverter : ValueConverter<bool, BoolNode>
    {
        /* Protected methods. */
        protected override BoolNode Convert(bool obj, Context context) => new(obj);
        protected override bool Deconvert(BoolNode node, Context context) => node.Value;
    }
}