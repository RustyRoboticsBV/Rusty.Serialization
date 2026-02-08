#if NETCOREAPP3_0_OR_GREATER
using System;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET index converter.
    /// </summary>
    public class IndexConverter : Core.Conversion.Converter<Index, IntNode>
    {
        /* Protected method. */
        protected override IntNode CreateNode(Index obj, CreateNodeContext context) => new IntNode(obj.Value);

        protected override Index CreateObject(IntNode node, CreateObjectContext context) => new Index((int)node.Value);
    }
}
#endif