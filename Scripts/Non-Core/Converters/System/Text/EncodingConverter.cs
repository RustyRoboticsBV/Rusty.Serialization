using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Text.Encoding converter.
    /// </summary>
    public sealed class EncodingConverter : ReferenceConverter<Encoding, StringNode>
    {
        /* Protected methods. */
        protected override StringNode ConvertRef(Encoding obj, IConverterScheme scheme) => new(obj.WebName);
        protected override Encoding DeconvertRef(StringNode node, IConverterScheme scheme) => Encoding.GetEncoding(node.Value);
    }
}