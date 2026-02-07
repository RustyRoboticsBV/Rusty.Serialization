using System;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET guid converter.
    /// </summary>
    public class GuidConverter : Core.Conversion.Converter<Guid, BytesNode>
    {
        /* Protected method. */
        protected override BytesNode CreateNode(Guid obj, CreateNodeContext context)
        {
            return new BytesNode(obj.ToByteArray());
        }

        protected override Guid CreateObject(BytesNode node, CreateObjectContext context)
        {
            return new Guid((byte[])node.Name);
        }
    }
}