using System;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET guid converter.
    /// </summary>
    public class GuidConverter : Core.Conversion.Converter<Guid, UidNode>
    {
        /* Protected method. */
        protected override UidNode CreateNode(Guid obj, CreateNodeContext context)
        {
            return new UidNode(obj);
        }

        protected override Guid CreateObject(UidNode node, CreateObjectContext context)
        {
            return node.Value;
        }
    }
}