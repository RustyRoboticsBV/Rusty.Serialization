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
        protected override StringNode CreateNode(Encoding obj, IConverterScheme scheme, SymbolTable table) => new(obj.WebName);
        protected override Encoding CreateObject(StringNode node, IConverterScheme scheme, NodeTree tree) => Encoding.GetEncoding(node.Value);
    }
}