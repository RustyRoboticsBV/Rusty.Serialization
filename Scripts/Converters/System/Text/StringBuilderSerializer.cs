using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A System.Text.StringBuilder converter.
    /// </summary>
    public sealed class StringBuilderConverter : ReferenceConverter<StringBuilder, StringNode>
    {
        /* Protected methods. */
        protected override StringNode Convert(StringBuilder obj, Context context) => new(obj.ToString());
        protected override StringBuilder Deconvert(StringNode node, Context context) => new(node.Value);
    }
}