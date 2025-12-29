using System;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET guid converter.
    /// </summary>
    public class GuidConverter : Core.Converters.Converter<Guid, StringNode>
    {
        /* Protected method. */
        protected override StringNode CreateNode(Guid obj, CreateNodeContext context)
        {
            return new StringNode(obj.ToString());
        }

        protected override Guid CreateObject(StringNode node, CreateObjectContext context)
        {
            return Guid.Parse(node.Value);
        }
    }
}