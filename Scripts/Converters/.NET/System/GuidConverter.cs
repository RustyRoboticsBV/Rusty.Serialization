using System;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET guid converter.
    /// </summary>
    public class GuidConverter : Core.Converters.Converter<Guid, BinaryNode>
    {
        /* Protected method. */
        protected override BinaryNode CreateNode(Guid obj, CreateNodeContext context)
        {
            return new BinaryNode(obj.ToByteArray());
        }

        protected override Guid CreateObject(BinaryNode node, CreateObjectContext context)
        {
            return new Guid(node.Value);
        }
    }
}