using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Text.StringBuilder converter.
    /// </summary>
    public sealed class StringBuilderConverter : ReferenceConverter<StringBuilder, StringNode>
    {
        /* Protected methods. */
        protected override StringNode ConvertRef(StringBuilder obj, IConverterScheme scheme) => new(obj.ToString());
        protected override StringBuilder DeconvertRef(StringNode node, IConverterScheme scheme) => new(node.Value);
    }
}