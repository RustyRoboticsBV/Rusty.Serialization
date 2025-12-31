#if NET5_0_OR_GREATER
using System;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET half converter.
    /// </summary>
    public class HalfConverter : Core.Converters.Converter<Half, RealNode>
    {
        /* Protected method. */
        protected override RealNode CreateNode(Half obj, CreateNodeContext context)
        {
            return new RealNode((float)obj);
        }

        protected override Half CreateObject(RealNode node, CreateObjectContext context)
        {
            return Half.Parse(node.Value);
        }
    }
}
#endif