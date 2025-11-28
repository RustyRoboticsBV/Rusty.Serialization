using Rusty.Serialization.Core.Contexts;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An int converter.
    /// </summary>
    public sealed class IntConverter : ValueConverter<int, IntNode>
    {
        /* Protected methods. */
        protected override IntNode Convert(int obj, Context context) => new(obj);
        protected override int Deconvert(IntNode node, Context context) => (int)node.Value;
    }
}