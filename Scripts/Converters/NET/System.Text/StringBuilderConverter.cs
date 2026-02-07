using System.Text;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET string builder converter.
    /// </summary>
    public class StringBuilderConverter : Converter<StringBuilder, StringNode>
    {
        /* Protected method. */
        protected override StringNode CreateNode(StringBuilder obj, CreateNodeContext context)
        {
            return new StringNode(obj.ToString());
        }

        protected override StringBuilder CreateObject(StringNode node, CreateObjectContext context)
        {
            return new StringBuilder(node.Name);
        }
    }
}