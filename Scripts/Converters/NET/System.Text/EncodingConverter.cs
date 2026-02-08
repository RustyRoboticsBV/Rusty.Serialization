using System.Text;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET encoding converter.
    /// </summary>
    public class EncodingConverter : Converter<Encoding, StringNode>
    {
        /* Protected method. */
        protected override StringNode CreateNode(Encoding obj, CreateNodeContext context)
        {
            return new StringNode(obj.WebName);
        }

        protected override Encoding CreateObject(StringNode node, CreateObjectContext context)
        {
            return Encoding.GetEncoding(node.Value);
        }
    }
}