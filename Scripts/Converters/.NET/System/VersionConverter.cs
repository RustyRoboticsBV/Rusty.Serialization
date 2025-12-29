using System;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET version converter.
    /// </summary>
    public class VersionConverter : Core.Converters.Converter<Version, StringNode>
    {
        /* Protected method. */
        protected override StringNode CreateNode(Version obj, CreateNodeContext context)
        {
            return new StringNode(obj.ToString());
        }

        protected override Version CreateObject(StringNode node, CreateObjectContext context)
        {
            return Version.Parse(node.Value);
        }
    }
}